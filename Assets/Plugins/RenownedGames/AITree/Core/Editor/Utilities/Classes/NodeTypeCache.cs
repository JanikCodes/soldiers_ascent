﻿/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.AITree;
using RenownedGames.ExLibEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RenownedGames.AITreeEditor
{
    [InitializeOnLoad]
    public static class NodeTypeCache
    {
        public readonly struct NodeInfo
        {
            public readonly Type type;
            public readonly NodeContentAttribute attribute;
            public readonly Texture2D icon;

            internal NodeInfo(Type type, NodeContentAttribute attribute)
            {
                this.type = type;
                this.attribute = attribute;
                icon = null;

                string iconPath = attribute?.IconPath ?? string.Empty;

                if (string.IsNullOrEmpty(iconPath) && type.IsSubclassOf(typeof(TaskNode)))
                {
                    const string DEFAULT_TASK_ICON_PATH = "Images/Icons/Node/TaskIcon.png";
                    iconPath = DEFAULT_TASK_ICON_PATH;
                }

                if (!string.IsNullOrEmpty(iconPath))
                {
                    if (iconPath[0] == '@')
                    {
                        iconPath = iconPath.Remove(0, 1);
                        icon = EditorGUIUtility.IconContent(iconPath).image as Texture2D;
                    }
                    else
                    {
                        icon = EditorResources.Load<Texture2D>(iconPath);
                    }
                }
            }
        }

        public readonly struct NodeCollection : IEnumerable<NodeInfo>, IReadOnlyCollection<NodeInfo>
        {
            private readonly List<NodeInfo> nodes;

            internal NodeCollection(List<NodeInfo> nodes)
            {
                this.nodes = nodes;
            }

            public NodeInfo this[int index]
            {
                get
                {
                    return nodes[index];
                }
            }

            #region [IReadOnlyCollection<NodeType> Implementation]
            public int Count
            {
                get
                {
                    return nodes.Count;
                }
            }
            #endregion

            #region [IEnumerable<NodeType> Implementation]
            public IEnumerator<NodeInfo> GetEnumerator()
            {
                return nodes.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return nodes.GetEnumerator();
            }
            #endregion
        }

        private static NodeCollection nodesInfo;

        static NodeTypeCache()
        {
            const string GUID = "NodeTypeCache_EditorLaunch";
            if (!SessionState.GetBool(GUID, false))
            {
                EditorApplication.delayCall += Load;
                SessionState.SetBool(GUID, true);
            }
            else
            {
                Load();
            }
        }

        private static void Load()
        {
            StackTraceLogType stackTraceLogType = Application.GetStackTraceLogType(LogType.Warning);
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
            TypeCache.TypeCollection nodeImpls = TypeCache.GetTypesDerivedFrom<Node>();
            List<NodeInfo> nodeTypes = new List<NodeInfo>(nodeImpls.Count);
            for (int i = 0; i < nodeImpls.Count; i++)
            {
                Type nodeImpl = nodeImpls[i];
                if (nodeImpl.IsAbstract)
                {
                    continue;
                }

                ScriptableObject clone = ScriptableObject.CreateInstance(nodeImpl);
                MonoScript monoScript = MonoScript.FromScriptableObject(clone);
                if (monoScript == null)
                {
                    Debug.LogWarning($"No script asset for {nodeImpl.Name}. Check that the definition is in a file of the same name and that it compiles properly.");
                    continue;
                }
                UnityEngine.Object.DestroyImmediate(clone);

                NodeContentAttribute attribute = nodeImpl.GetCustomAttribute<NodeContentAttribute>(false);
                nodeTypes.Add(new NodeInfo(nodeImpl, attribute));
            }
            nodesInfo = new NodeCollection(nodeTypes);
            Application.SetStackTraceLogType(LogType.Warning, stackTraceLogType);
        }

        #region [Getter / Setter]
        public static NodeCollection GetNodesInfo()
        {
            return nodesInfo;
        }
        #endregion
    }
}
