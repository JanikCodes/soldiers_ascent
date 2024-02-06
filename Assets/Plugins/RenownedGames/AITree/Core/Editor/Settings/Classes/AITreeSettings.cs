/* ================================================================
   ----------------------------------------------------------------
   Project   :   AI Tree
   Publisher :   Renowned Games
   Developer :   Tamerlan Shakirov
   ----------------------------------------------------------------
   Copyright 2022-2023 Renowned Games All rights reserved.
   ================================================================ */

using RenownedGames.Apex;
using RenownedGames.ExLibEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RenownedGames.AITreeEditor
{
    [HideMonoScript]
    [UnityEditor.FilePath("ProjectSettings/AITreeSettings.asset", UnityEditor.FilePathAttribute.Location.ProjectFolder)]
    public sealed class AITreeSettings : ScriptableSingleton<AITreeSettings>
    {
        public enum HotKeyAPI
        {
            KeyDownListener,
            KeyDownEvent
        }

        [SerializeField]
        private bool allowNodeSnapping = false;

        [SerializeField]
        private bool showMiniMap = false;

        [SerializeField]
        private HotKeyAPI graphHotKeyAPI = HotKeyAPI.KeyDownEvent;

        [SerializeField]
        [Title("Advanced")]
        [ShowIf("advanced")]
        [NotNull]
        private VisualTreeAsset behaviourTreeUXML;

        [SerializeField]
        [ShowIf("advanced")]
        [NotNull]
        private StyleSheet behaviourTreeUSS;

        [SerializeField]
        [ShowIf("advanced")]
        [NotNull]
        private VisualTreeAsset nodeUXML;

        [SerializeField]
        [ShowIf("advanced")]
        [NotNull]
        private StyleSheet nodeUSS;

        [SerializeField]
        [ShowIf("advanced")]
        [NotNull]
        private VisualTreeAsset decoratorUXML;

        [SerializeField]
        [ShowIf("advanced")]
        [NotNull]
        private VisualTreeAsset serviceUXML;

        [SerializeField]
        [ShowIf("advanced")]
        [NotNull]
        private VisualTreeAsset blackboardUXML;

        [SerializeField]
        [ShowIf("advanced")]
        [NotNull]
        private StyleSheet blackboardUSS;

        [SerializeField]
        [ShowIf("advanced")]
        [NotNull]
        private VisualTreeAsset blackboardDetailsUXML;

        [SerializeField]
        [ShowIf("advanced")]
        [NotNull]
        private StyleSheet blackboardDetailsUSS;

        [SerializeField]
        [HideInInspector]
        [ShowIf("advanced")]
        [NotNull]
        private VisualTreeAsset keyDetailsUXML;

        [SerializeField]
        [HideInInspector]
        [ShowIf("advanced")]
        [NotNull]
        private StyleSheet keyDetailsUSS;

        [SerializeField]
        [HideInInspector]
        internal bool advanced;

        /// <summary>
        /// Save all changed data.
        /// </summary>
        public void Save()
        {
            Save(true);
        }

        /// <summary>
        /// Called when the user hits the Reset button in the Inspector's context menu 
        /// or when creating settings.
        /// </summary>
        public void Reset()
        {
            allowNodeSnapping = false;
            showMiniMap = false;
#if UNITY_2023
            graphHotKeyAPI = HotKeyAPI.KeyDownListener;
#else
            graphHotKeyAPI = HotKeyAPI.KeyDownEvent;
#endif
            LoadDefaultStyles();
            Save();
        }

        /// <summary>
        /// Load the default built-in styles.
        /// </summary>
        public void LoadDefaultStyles()
        {
            behaviourTreeUXML = EditorResources.Load<VisualTreeAsset>("Styles/BehaviourTreeUXML.uxml");
            behaviourTreeUSS = EditorResources.Load<StyleSheet>("Styles/BehaviourTreeUSS.uss");
            nodeUXML = EditorResources.Load<VisualTreeAsset>("Styles/NodeUXML.uxml");
            nodeUSS = EditorResources.Load<StyleSheet>("Styles/NodeUSS.uss");
            decoratorUXML = EditorResources.Load<VisualTreeAsset>("Styles/DecoratorUXML.uxml");
            serviceUXML = EditorResources.Load<VisualTreeAsset>("Styles/ServiceUXML.uxml");
            blackboardUXML = EditorResources.Load<VisualTreeAsset>("Styles/BlackboardUXML.uxml");
            blackboardUSS = EditorResources.Load<StyleSheet>("Styles/BlackboardUSS.uss");
            blackboardDetailsUXML = EditorResources.Load<VisualTreeAsset>("Styles/BlackboardDetailsUXML.uxml");
            blackboardDetailsUSS = EditorResources.Load<StyleSheet>("Styles/BlackboardDetailsUSS.uss");
            keyDetailsUXML = EditorResources.Load<VisualTreeAsset>("Styles/KeyDetailsUXML.uxml");
            keyDetailsUSS = EditorResources.Load<StyleSheet>("Styles/KeyDetailsUSS.uss");
        }

        /// <summary>
        /// Check if all required styles has been loaded in settings.
        /// </summary>
        public bool IsStylesLoaded()
        {
            return
                behaviourTreeUXML != null &&
                behaviourTreeUSS != null &&
                nodeUXML != null &&
                nodeUSS != null &&
                decoratorUXML != null &&
                serviceUXML != null &&
                blackboardUXML != null &&
                blackboardUSS != null &&
                blackboardDetailsUXML != null &&
                blackboardDetailsUSS != null &&
                keyDetailsUXML != null &&
                keyDetailsUSS != null;
        }

        #region [Static Method]
        [System.Obsolete("Use AITreeSettings.instance property instead.")]
        public static AITreeSettings Current
        {
            get
            {
                const string GUID = "AI Tree Settings Object";
                if (!EditorBuildSettings.TryGetConfigObject<AITreeSettings>(GUID, out AITreeSettings settings))
                {
                    AITreeSettings[] allSettings = ProjectDatabase.LoadAll<AITreeSettings>();
                    if (allSettings.Length > 0)
                    {
                        settings = allSettings[0];
                    }
                    else
                    {
                        settings = CreateInstance<AITreeSettings>();
                        settings.name = "AITreeSettings";

                        string path = AssetDatabase.GenerateUniqueAssetPath($"Assets/{settings.name}.asset");
                        AssetDatabase.CreateAsset(settings, path);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }

                    EditorBuildSettings.AddConfigObject(GUID, settings, true);
                }
                return settings;
            }
        }
        #endregion

        #region [Getter / Setter]
        public bool AllowNodeSnapping()
        {
            return allowNodeSnapping;
        }

        public void AllowNodeSnapping(bool value)
        {
            allowNodeSnapping = value;
        }

        public bool ShowMiniMap()
        {
            return showMiniMap;
        }

        public void ShowMiniMap(bool value)
        {
            showMiniMap = value;
        }

        public HotKeyAPI GetGraphHotKeyAPI()
        {
            return graphHotKeyAPI;
        }

        public void SetGraphHotKeyAPI(HotKeyAPI value)
        {
            graphHotKeyAPI = value;
        }

        public VisualTreeAsset GetBehaviourTreeUXML()
        {
            if (behaviourTreeUXML == null)
            {
                behaviourTreeUXML = EditorResources.Load<VisualTreeAsset>("Styles/BehaviourTreeUXML.uxml");
                Save();
            }
            return behaviourTreeUXML;
        }

        public void SetBehaviourTreeUXML(VisualTreeAsset value)
        {
            behaviourTreeUXML = value;
        }

        public StyleSheet GetBehaviourTreeUSS()
        {
            if (behaviourTreeUSS == null)
            {
                behaviourTreeUSS = EditorResources.Load<StyleSheet>("Styles/BehaviourTreeUSS.uss");
                Save();
            }
            return behaviourTreeUSS;
        }

        public void SetBehaviourTreeUSS(StyleSheet value)
        {
            behaviourTreeUSS = value;
        }

        public VisualTreeAsset GetNodeUXML()
        {
            if (nodeUXML == null)
            {
                nodeUXML = EditorResources.Load<VisualTreeAsset>("Styles/NodeUXML.uxml");
                Save();
            }
            return nodeUXML;
        }

        public void SetNodeUXML(VisualTreeAsset value)
        {
            nodeUXML = value;
        }

        public StyleSheet GetNodeUSS()
        {
            if (nodeUSS == null)
            {
                nodeUSS = EditorResources.Load<StyleSheet>("Styles/NodeUSS.uss");
                Save();
            }
            return nodeUSS;
        }

        public void SetNodeUSS(StyleSheet value)
        {
            nodeUSS = value;
        }

        public VisualTreeAsset GetDecoratorUXML()
        {
            if (decoratorUXML == null)
            {
                decoratorUXML = EditorResources.Load<VisualTreeAsset>("Styles/DecoratorUXML.uxml");
                Save();
            }
            return decoratorUXML;
        }

        public void SetDecoratorUXML(VisualTreeAsset value)
        {
            decoratorUXML = value;
        }

        public VisualTreeAsset GetServiceUXML()
        {
            if (serviceUXML == null)
            {
                serviceUXML = EditorResources.Load<VisualTreeAsset>("Styles/ServiceUXML.uxml");
                Save();
            }
            return serviceUXML;
        }

        public void SetServiceUXML(VisualTreeAsset value)
        {
            serviceUXML = value;
        }

        public VisualTreeAsset GetBlackboardUXML()
        {
            if (blackboardUXML == null)
            {
                blackboardUXML = EditorResources.Load<VisualTreeAsset>("Styles/BlackboardUXML.uxml");
                Save();
            }
            return blackboardUXML;
        }

        public void SetBlackboardUXML(VisualTreeAsset value)
        {
            blackboardUXML = value;
        }

        public StyleSheet GetBlackboardUSS()
        {
            if (blackboardUSS == null)
            {
                blackboardUSS = EditorResources.Load<StyleSheet>("Styles/BlackboardUSS.uss");
                Save();
            }
            return blackboardUSS;
        }

        public void SetBlackboardUSS(StyleSheet value)
        {
            blackboardUSS = value;
        }

        public VisualTreeAsset GetBlackboardDetailsUXML()
        {
            if (blackboardDetailsUXML == null)
            {
                blackboardDetailsUXML = EditorResources.Load<VisualTreeAsset>("Styles/BlackboardDetailsUXML.uxml");
                Save();
            }
            return blackboardDetailsUXML;
        }

        public void SetBlackboardDetailsUXML(VisualTreeAsset value)
        {
            blackboardDetailsUXML = value;
        }

        public StyleSheet GetBlackboardDetailsUSS()
        {
            if (blackboardDetailsUSS == null)
            {
                blackboardDetailsUSS = EditorResources.Load<StyleSheet>("Styles/BlackboardDetailsUSS.uss");
                Save();
            }
            return blackboardDetailsUSS;
        }

        public void SetBlackboardDetailsUSS(StyleSheet value)
        {
            blackboardDetailsUSS = value;
        }

        [System.Obsolete]
        public VisualTreeAsset GetKeyDetailsUXML()
        {
            if (keyDetailsUXML == null)
            {
                keyDetailsUXML = EditorResources.Load<VisualTreeAsset>("Styles/KeyDetailsUXML.uxml");
                Save();
            }
            return keyDetailsUXML;
        }

        [System.Obsolete]
        public void SetKeyDetailsUXML(VisualTreeAsset value)
        {
            keyDetailsUXML = value;
        }

        [System.Obsolete]
        public StyleSheet GetKeyDetailsUSS()
        {
            if (keyDetailsUSS == null)
            {
                keyDetailsUSS = EditorResources.Load<StyleSheet>("Styles/KeyDetailsUSS.uss");
                Save();
            }
            return keyDetailsUSS;
        }

        [System.Obsolete]
        public void SetKeyDetailsUSS(StyleSheet value)
        {
            keyDetailsUSS = value;
        }
        #endregion
    }
}