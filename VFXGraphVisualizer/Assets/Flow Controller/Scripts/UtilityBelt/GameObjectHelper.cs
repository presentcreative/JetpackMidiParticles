/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.UtilityBelt
{
    using System;
    using UnityEngine;

    public class GameObjectHelper
    {
        /// <summary>
        /// Find immediate child, i.e. no grandchildren.
        /// </summary>
        public static GameObject FindImmediate(Type t, string name, GameObject parent, Component[] components = null)
        {
            if(components == null)
            {
                components = parent.GetComponentsInChildren(t);
            }

            foreach (Component component in components)
            {
                if(name == component.gameObject.name) {
                    return component.gameObject;
                }
            }

            return null;
        }

        public static string GenerateNextName(Type t, string prefix, GameObject parent)
        {
            Component[] components = parent.GetComponentsInChildren(t);
            int next = components.Length + 1;
            while(FindImmediate(t, prefix + @" " + next, parent, components) != null)
            {
                next++;
            }
            return prefix + @" " + next;
        }
    }
}
