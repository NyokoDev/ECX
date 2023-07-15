using UnifiedUI.Helpers;
using ColossalFramework;
using UnityEngine;

namespace ToggleKey
{
    internal class Concrete2InputKey : UnsavedInputKey
    {
        private string v1;
        private string v2;
        private KeyCode space;

        public Concrete2InputKey(string keyName, string modName, InputKey key) : base(keyName, modName, key)
        {
        }

       

        // You can add any additional properties or methods specific to InputKeys if needed

        public override void OnConflictResolved()
        {
            // Handle conflicts when multiple keys have the same binding
            Debug.Log("Conflict resolved for YourBindingName");

            // Add your conflict resolution logic here
            // For example, you can display a message, disable conflicting inputs, or prompt the user to rebind the key
        }
    }
}
