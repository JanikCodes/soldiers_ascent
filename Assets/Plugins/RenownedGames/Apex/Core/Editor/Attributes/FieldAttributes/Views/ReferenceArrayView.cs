﻿/* ================================================================
  ---------------------------------------------------
  Project   :    Apex
  Publisher :    Renowned Games
  Developer :    Tamerlan Shakirov
  ---------------------------------------------------
  Copyright 2020-2023 Renowned Games All rights reserved.
  ================================================================ */

using RenownedGames.Apex;
using RenownedGames.ExLib.Reflection;
using RenownedGames.ExLibEditor.Windows;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vexe.Runtime.Extensions;
using Object = UnityEngine.Object;

namespace RenownedGames.ApexEditor
{
    [ViewTarget(typeof(ReferenceArrayAttribute))]
    public sealed class ReferenceArrayView : FieldView, ITypeValidationCallback
    {
        private SerializedField serializedField;
        private ReorderableArray reorderableArray;
        private ReferenceArrayAttribute attribute;

        private Action<SerializedProperty> onReorder;
        private Action<SerializedProperty, int> onAdd;
        private Action<SerializedProperty, int> onRemove;
        private Action<Rect, SerializedProperty, GUIContent> onGUI;
        private Func<SerializedProperty, int, float> getHeight;
        private Func<SerializedProperty, int, GUIContent> getLabel;
        private Func<Type, bool> typeFilter;

        /// <summary>
        /// Called once when initializing PropertyView.
        /// </summary>
        /// <param name="serializedField">Serialized field with ViewAttribute.</param>
        /// <param name="viewAttribute">ViewAttribute of Serialized field.</param>
        /// <param name="label">Label of Serialized field.</param>
        public override void Initialize(SerializedField serializedField, ViewAttribute viewAttribute, GUIContent label)
        {
            attribute = viewAttribute as ReferenceArrayAttribute;
            FindCallbacks(serializedField);
            OverrideElementsLabel(serializedField);

            this.serializedField = serializedField;
            reorderableArray = new ReorderableArray(serializedField, true)
            {
                onElementGUI = OnElementGUI,
                getElementHeight = GetElementHeight,
                onAddClick = OnAddElement,
                onRemoveClick = OnRemoveElement,
                onReorder = OnReorderList
            };
        }

        /// <summary>
        /// Called for drawing serializedField view GUI.
        /// </summary>
        /// <param name="position">Position of the serialized serializedField.</param>
        /// <param name="serializedField">Serialized serializedField with ViewAttribute.</param>
        /// <param name="label">Label of serialized serializedField.</param>
        public override void OnGUI(Rect position, SerializedField serializedField, GUIContent label)
        {
            reorderableArray.Draw(EditorGUI.IndentedRect(position));
        }

        /// <summary>
        /// Get height which needed to draw property.
        /// </summary>
        /// <param name="property">Serialized serializedField with ViewAttribute.</param>
        /// <param name="label">Label of serialized serializedField.</param>
        public override float GetHeight(SerializedField serializedField, GUIContent label)
        {
            return reorderableArray.GetHeight();
        }

        /// <summary>
        /// Return true if this property valid the using with this attribute.
        /// If return false, this property attribute will be ignored.
        /// </summary>
        /// <param name="property">Reference of serialized property.</param>
        /// <param name="label">Display label of serialized property.</param>
        public bool IsValidProperty(SerializedProperty property)
        {
            return property.isArray
                && property.propertyType == SerializedPropertyType.Generic;
        }

        /// <summary>
        /// Called to draw element of array.
        /// </summary>
        private void OnElementGUI(Rect position, int index, bool isFocused, bool isActive)
        {
            SerializedField field = serializedField.GetArrayElement(index);
            if (onGUI != null)
            {
                onGUI.Invoke(position, field.GetSerializedProperty(), field.GetLabel());
            }
            else
            {
                field.OnGUI(position);
            }
        }

        /// <summary>
        /// Called to calculate height of array element.
        /// </summary>
        private float GetElementHeight(int index)
        {
            if (getHeight != null)
            {
                return getHeight.Invoke(serializedField.GetSerializedProperty(), index);
            }
            else
            {
                return serializedField.GetArrayElement(index).GetHeight();
            }
        }

        /// <summary>
        /// Called to add new element to array.
        /// </summary>
        private void OnAddElement(Rect position)
        {
            DropdownTypes(position, serializedField);
        }

        /// <summary>
        /// Called to remove selected element from array.
        /// </summary>
        private void OnRemoveElement(Rect position, int index)
        {
            onRemove?.Invoke(serializedField.GetSerializedProperty(), index);
            serializedField.RemoveArrayElement(index);
            OverrideElementsLabel(serializedField);
        }

        /// <summary>
        /// Called when list reordered.
        /// </summary>
        private void OnReorderList()
        {
            OverrideElementsLabel(serializedField);
            onReorder?.Invoke(serializedField.GetSerializedProperty());
        }

        /// <summary>
        /// Override elements label by label callback.
        /// </summary>
        /// <param name="serializedField">Serialzied field of array.</param>
        private void OverrideElementsLabel(SerializedField serializedField)
        {
            if(getLabel == null && attribute.ArrayNaming)
            {
                return;
            }

            SerializedProperty array = serializedField.GetSerializedProperty();
            for (int i = 0; i < serializedField.GetArrayLength(); i++)
            {
                SerializedField field = serializedField.GetArrayElement(i);
                if(getLabel != null) 
                {
                    field.SetLabel(getLabel.Invoke(array, i));
                }
                else
                {
                    object value = field.GetManagedReference();
                    if(value != null)
                    {
                        Type type = value.GetType();
                        SearchContent attribute = type.GetCustomAttribute<SearchContent>();
                        if (attribute != null)
                        {
                            GUIContent content = new GUIContent(System.IO.Path.GetFileName(attribute.name), attribute.Tooltip);
                            if (SearchContentUtility.TryLoadContentImage(attribute.Image, out Texture2D icon))
                            {
                                content.image = icon;
                            }
                            field.SetLabel(content);
                        }
                        else
                        {
                            field.SetLabel(new GUIContent(ObjectNames.NicifyVariableName(type.Name), field.GetSerializedProperty().tooltip));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Show search window with all derived references.
        /// </summary>
        /// <param name="position">Button position.</param>
        /// <param name="serializedField">Serialized field reference.</param>
        private void DropdownTypes(Rect position, SerializedField serializedField)
        {
            string title = string.IsNullOrWhiteSpace(attribute.DropdownTitle) ? "Types" : attribute.DropdownTitle;
            ExSearchWindow searchWindow = ExSearchWindow.Create(title);

            Type memberType = serializedField.GetMemberType();
            Type elementType = memberType.GetElementType();
            if (memberType.IsGenericType)
            {
                elementType = memberType.GetGenericArguments()[0];
            }

            TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom(elementType);
            for (int i = 0; i < types.Count; i++)
            {
                Type type = types[i];
                if (type.IsAbstract || !type.HasDefaultConstructor())
                {
                    continue;
                }

                if (attribute.IncludeTypes != null && !attribute.IncludeTypes.Contains(type))
                {
                    continue;
                }

                if (attribute.ExcludeTypes != null && attribute.ExcludeTypes.Contains(type))
                {
                    continue;
                }

                if(typeFilter != null && !typeFilter.Invoke(type))
                {
                    continue;
                }

                GUIContent content = new GUIContent(ObjectNames.NicifyVariableName(type.Name));
                SearchContent searchContent = type.GetCustomAttribute<SearchContent>();
                if (searchContent != null)
                {
                    if (searchContent.Hidden)
                    {
                        continue;
                    }

                    content.text = searchContent.name;
                    content.tooltip = searchContent.Tooltip;
                    if (SearchContentUtility.TryLoadContentImage(searchContent.Image, out Texture2D image))
                    {
                        content.image = image;
                    }
                }

                searchWindow.AddEntry(content, () => CreateInstance(type, serializedField));
            }
            searchWindow.Open(position);

            void CreateInstance(Type type, SerializedField serializedField)
            {
                SerializedProperty property = serializedField.GetSerializedProperty();

                int index = property.arraySize;
                property.arraySize++;

                SerializedProperty element = property.GetArrayElementAtIndex(index);
                element.managedReferenceValue = Activator.CreateInstance(type);
                element.serializedObject.ApplyModifiedProperties();

                OverrideElementsLabel(serializedField);
                onAdd?.Invoke(property, index);
            }
        }

        /// <summary>
        /// Find callbacks of reference array view.
        /// </summary>
        /// <param name="serializedField">Serialized field of array.</param>
        /// <param name="attribute">Array view attribute.</param>
        private void FindCallbacks(SerializedField serializedField)
        {
            object target = serializedField.GetDeclaringObject();
            Type type = target.GetType();
            Type limitDescendant = target is MonoBehaviour ? typeof(MonoBehaviour) : typeof(Object);
            foreach (MethodInfo methodInfo in type.AllMethods(limitDescendant))
            {
                if (onGUI == null && methodInfo.IsValidCallback(attribute.OnGUI, typeof(void), typeof(Rect), typeof(SerializedProperty), typeof(GUIContent)))
                {
                    onGUI = (Action<Rect, SerializedProperty, GUIContent>)methodInfo.CreateDelegate(typeof(Action<Rect, SerializedProperty, GUIContent>), target);
                    continue;
                }

                if (getHeight == null && methodInfo.IsValidCallback(attribute.GetHeight, typeof(float), typeof(SerializedProperty), typeof(int)))
                {
                    getHeight = (Func<SerializedProperty, int, float>)methodInfo.CreateDelegate(typeof(Func<SerializedProperty, int, float>), target);
                    continue;
                }

                if (getLabel == null && methodInfo.IsValidCallback(attribute.GetLabel, typeof(GUIContent), typeof(SerializedProperty), typeof(int)))
                {
                    getLabel = (Func<SerializedProperty, int, GUIContent>)methodInfo.CreateDelegate(typeof(Func<SerializedProperty, int, GUIContent>), target);
                    continue;
                }

                if (onAdd == null && methodInfo.IsValidCallback(attribute.OnAdd, typeof(void), typeof(SerializedProperty), typeof(int)))
                {
                    onAdd = (Action<SerializedProperty, int>)methodInfo.CreateDelegate(typeof(Action<SerializedProperty, int>), target);
                    continue;
                }

                if (onRemove == null && methodInfo.IsValidCallback(attribute.OnRemove, typeof(void), typeof(SerializedProperty), typeof(int)))
                {
                    onRemove = (Action<SerializedProperty, int>)methodInfo.CreateDelegate(typeof(Action<SerializedProperty, int>), target);
                    continue;
                }

                if (onReorder == null && methodInfo.IsValidCallback(attribute.OnReorder, typeof(void), typeof(SerializedProperty)))
                {
                    onReorder = (Action<SerializedProperty>)methodInfo.CreateDelegate(typeof(Action<SerializedProperty>), target);
                    continue;
                }

                if (typeFilter == null && methodInfo.IsValidCallback(attribute.TypeFilter, typeof(bool), typeof(Type)))
                {
                    typeFilter =  (Func<Type, bool>)methodInfo.CreateDelegate(typeof(Func<Type, bool>), target);
                    continue;
                }
            }
        }
    }
}