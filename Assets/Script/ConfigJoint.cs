// CharacterJoint[] characterJoints = FindObjectsByType<CharacterJoint>(FindObjectsSortMode.None);
/*using UnityEngine;
using UnityEditor;

public class ConvertCharacterJoints : EditorWindow
{
    [MenuItem("Tools/Convert Character Joints to Configurable Joints")]
    static void ConvertAllCharacterJointsInScene()
    {
         CharacterJoint[] characterJoints = FindObjectsByType<CharacterJoint>(FindObjectsSortMode.None);
        int convertedCount = 0;

        foreach (CharacterJoint cj in characterJoints)
        {
            // --- 1. Store references/values in local variables ---
            GameObject go = cj.gameObject;
            Rigidbody connectedBody = cj.connectedBody;
            Vector3 anchor = cj.anchor;
            bool autoConfigureConnectedAnchor = cj.autoConfigureConnectedAnchor;
            Vector3 connectedAnchor = cj.connectedAnchor;
            Vector3 axis = cj.axis;

            // Limits
            SoftJointLimit lowTwist = cj.lowTwistLimit;
            SoftJointLimit highTwist = cj.highTwistLimit;
            SoftJointLimit swing1 = cj.swing1Limit;
            SoftJointLimit swing2 = cj.swing2Limit;

            // Springs
            SoftJointLimitSpring twistSpring = cj.twistLimitSpring;
            SoftJointLimitSpring swingSpring = cj.swingLimitSpring;

            // Misc
            float breakForce = cj.breakForce;
            float breakTorque = cj.breakTorque;
            float massScale = cj.massScale;
            float connectedMassScale = cj.connectedMassScale;

            // --- 2. Destroy the CharacterJoint ---
            DestroyImmediate(cj);

            // --- 3. Add a ConfigurableJoint ---
            ConfigurableJoint confJoint = go.AddComponent<ConfigurableJoint>();

            // --- 4. Assign the stored properties ---
            confJoint.connectedBody = connectedBody;
            confJoint.anchor = anchor;
            confJoint.autoConfigureConnectedAnchor = autoConfigureConnectedAnchor;
            if (!autoConfigureConnectedAnchor)
            {
                confJoint.connectedAnchor = connectedAnchor;
            }

            // If you want to replicate the axis orientation, you can do:
            // confJoint.axis = axis;
            // But note that ConfigurableJoint handles axes differently from CharacterJoint.
            // Often you'll rely on the default local X-axis for twist.

            // Enable "Limited" motion for angular axes (typical ragdoll usage)
            confJoint.angularXMotion = ConfigurableJointMotion.Limited;
            confJoint.angularYMotion = ConfigurableJointMotion.Limited;
            confJoint.angularZMotion = ConfigurableJointMotion.Limited;

            // Map twist (low/high) to Angular X
            SoftJointLimit lowX = new SoftJointLimit
            {
                limit = Mathf.Abs(lowTwist.limit),
                contactDistance = lowTwist.contactDistance
            };
            confJoint.lowAngularXLimit = lowX;

            SoftJointLimit highX = new SoftJointLimit
            {
                limit = highTwist.limit,
                contactDistance = highTwist.contactDistance
            };
            confJoint.highAngularXLimit = highX;

            // Map swing1 → Y, swing2 → Z
            SoftJointLimit yLimit = new SoftJointLimit
            {
                limit = swing1.limit,
                contactDistance = swing1.contactDistance
            };
            confJoint.angularYLimit = yLimit;

            SoftJointLimit zLimit = new SoftJointLimit
            {
                limit = swing2.limit,
                contactDistance = swing2.contactDistance
            };
            confJoint.angularZLimit = zLimit;

            // Springs
            confJoint.angularXLimitSpring = new SoftJointLimitSpring
            {
                spring = twistSpring.spring,
                damper = twistSpring.damper
            };
            confJoint.angularYZLimitSpring = new SoftJointLimitSpring
            {
                spring = swingSpring.spring,
                damper = swingSpring.damper
            };

            // Break forces, mass scales, etc.
            confJoint.breakForce = breakForce;
            confJoint.breakTorque = breakTorque;
            confJoint.massScale = massScale;
            confJoint.connectedMassScale = connectedMassScale;

            convertedCount++;
        }

        Debug.Log($"Converted {convertedCount} CharacterJoints to ConfigurableJoints.");
    }
}
*/
using UnityEngine;
using UnityEditor;

public class ConvertAndAddConfigurableJoints : EditorWindow
{
    [MenuItem("Tools/Joints/Convert and Add Configurable Joints")]
    static void ConvertAndAddCJ()
    {
        // 1. Ensure a GameObject is selected as the root.
        if (Selection.activeGameObject == null)
        {
            Debug.LogError("No GameObject selected. Please select the root of your character.");
            return;
        }

        GameObject root = Selection.activeGameObject;
        Debug.Log($"Starting conversion/addition of ConfigurableJoints under: {root.name}");

        // 2. Get all transforms under the root (including inactive ones)
        Transform[] allChildren = root.GetComponentsInChildren<Transform>(true);

        int convertedCount = 0;
        int addedCount = 0;

        // 3. Iterate over each transform in the hierarchy.
        foreach (Transform t in allChildren)
        {
            // Option: Skip the root if you don't want a joint there.
            if (t == root.transform)
                continue;

            // Skip if this object already has a ConfigurableJoint.
            if (t.GetComponent<ConfigurableJoint>() != null)
                continue;

            // Ensure the object has a Rigidbody (required for joints)
            Rigidbody rb = t.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = Undo.AddComponent<Rigidbody>(t.gameObject);
                rb.mass = 1f; // set a default mass
            }

            // Get parent's Rigidbody for joint connection.
            Rigidbody parentRb = null;
            if (t.parent != null)
            {
                parentRb = t.parent.GetComponent<Rigidbody>();
                // Optionally, if parent's Rigidbody is missing, you could add one here.
            }

            // 4. If the object has a CharacterJoint, convert it.
            CharacterJoint cj = t.GetComponent<CharacterJoint>();
            if (cj != null)
            {
                // --- Store CharacterJoint properties ---
                Vector3 anchor = cj.anchor;
                bool autoConfigureConnectedAnchor = cj.autoConfigureConnectedAnchor;
                Vector3 connectedAnchor = cj.connectedAnchor;
                Vector3 axis = cj.axis;

                // Limits
                SoftJointLimit lowTwist = cj.lowTwistLimit;
                SoftJointLimit highTwist = cj.highTwistLimit;
                SoftJointLimit swing1 = cj.swing1Limit;
                SoftJointLimit swing2 = cj.swing2Limit;

                // Springs
                SoftJointLimitSpring twistSpring = cj.twistLimitSpring;
                SoftJointLimitSpring swingSpring = cj.swingLimitSpring;

                // Miscellaneous settings
                float breakForce = cj.breakForce;
                float breakTorque = cj.breakTorque;
                float massScale = cj.massScale;
                float connectedMassScale = cj.connectedMassScale;

                // --- Remove the CharacterJoint ---
                Undo.DestroyObjectImmediate(cj);

                // --- Add a ConfigurableJoint and assign stored properties ---
                ConfigurableJoint confJoint = Undo.AddComponent<ConfigurableJoint>(t.gameObject);
                confJoint.connectedBody = parentRb;
                confJoint.anchor = anchor;
                confJoint.autoConfigureConnectedAnchor = autoConfigureConnectedAnchor;
                if (!autoConfigureConnectedAnchor)
                    confJoint.connectedAnchor = connectedAnchor;

                // Copy axis – note that ConfigurableJoint handles axes differently.
                confJoint.axis = axis;

                // Lock linear motion (typical for ragdoll setups)
                confJoint.xMotion = ConfigurableJointMotion.Locked;
                confJoint.yMotion = ConfigurableJointMotion.Locked;
                confJoint.zMotion = ConfigurableJointMotion.Locked;

                // Limit angular motion
                confJoint.angularXMotion = ConfigurableJointMotion.Limited;
                confJoint.angularYMotion = ConfigurableJointMotion.Limited;
                confJoint.angularZMotion = ConfigurableJointMotion.Limited;

                // Map twist limits to angular X limits.
                SoftJointLimit lowX = new SoftJointLimit
                {
                    limit = Mathf.Abs(lowTwist.limit),
                    contactDistance = lowTwist.contactDistance
                };
                confJoint.lowAngularXLimit = lowX;

                SoftJointLimit highX = new SoftJointLimit
                {
                    limit = highTwist.limit,
                    contactDistance = highTwist.contactDistance
                };
                confJoint.highAngularXLimit = highX;

                // Map swing limits: swing1 → angularY, swing2 → angularZ.
                SoftJointLimit yLimit = new SoftJointLimit
                {
                    limit = swing1.limit,
                    contactDistance = swing1.contactDistance
                };
                confJoint.angularYLimit = yLimit;

                SoftJointLimit zLimit = new SoftJointLimit
                {
                    limit = swing2.limit,
                    contactDistance = swing2.contactDistance
                };
                confJoint.angularZLimit = zLimit;

                // Springs for angular limits.
                confJoint.angularXLimitSpring = new SoftJointLimitSpring
                {
                    spring = twistSpring.spring,
                    damper = twistSpring.damper
                };
                confJoint.angularYZLimitSpring = new SoftJointLimitSpring
                {
                    spring = swingSpring.spring,
                    damper = swingSpring.damper
                };

                // Transfer break forces and mass scales.
                confJoint.breakForce = breakForce;
                confJoint.breakTorque = breakTorque;
                confJoint.massScale = massScale;
                confJoint.connectedMassScale = connectedMassScale;

                convertedCount++;
            }
            else
            {
                // 5. If there is no CharacterJoint, add a default ConfigurableJoint.
                ConfigurableJoint confJoint = Undo.AddComponent<ConfigurableJoint>(t.gameObject);
                confJoint.connectedBody = parentRb;
                // Default settings:
                confJoint.xMotion = ConfigurableJointMotion.Locked;
                confJoint.yMotion = ConfigurableJointMotion.Locked;
                confJoint.zMotion = ConfigurableJointMotion.Locked;
                confJoint.angularXMotion = ConfigurableJointMotion.Limited;
                confJoint.angularYMotion = ConfigurableJointMotion.Limited;
                confJoint.angularZMotion = ConfigurableJointMotion.Limited;
                SoftJointLimit defaultLimit = new SoftJointLimit { limit = 30f, contactDistance = 0.1f };
                confJoint.lowAngularXLimit = defaultLimit;
                confJoint.highAngularXLimit = defaultLimit;
                confJoint.angularYLimit = defaultLimit;
                confJoint.angularZLimit = defaultLimit;
                addedCount++;
            }
        }

        Debug.Log($"Conversion complete: {convertedCount} CharacterJoints converted, {addedCount} new ConfigurableJoints added.");
    }
}

