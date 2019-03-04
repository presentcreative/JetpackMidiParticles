using UnityEngine;

using System.Collections;

using UnityEditor; // Need this to access SerializedObject

public class ParticleScript : MonoBehaviour
{

    SerializedObject thisParticle; // This will be our modifiable particle system

    bool isChanging; // Used as a flag for a coroutine

    //Set these values in the inspector. Will modify angle and radius of Particle System

    public float MAX_ANGLE, MIN_ANGLE, MAX_RADIUS, MIN_RADIUS, transitionSpeed;

    void Start()

    {

        /* Initialize and Assign thisParticle as a SerializedObject that takes properties

         * from the ParticleSystem attached to this game object. */

        thisParticle = new SerializedObject(GetComponent<ParticleSystem>());

        thisParticle.FindProperty("ShapeModule.radius").floatValue = MAX_RADIUS;

        thisParticle.FindProperty("ShapeModule.angle").floatValue = MAX_ANGLE;

        thisParticle.ApplyModifiedProperties(); // This basically updates the particles with any changes that have been made

        isChanging = false;

    }

    void Update()

    {

        if (Input.GetKeyDown(KeyCode.R) && !isChanging)

            StartCoroutine(ChangeRadius());

        else if (Input.GetKeyDown(KeyCode.A) && !isChanging)

            StartCoroutine(ChangeAngle());

    }

    IEnumerator ChangeRadius()

    {

        isChanging = true; // set true so user can't spam the coroutine

        //This code will make the radius smaller if the radius is at its maximum already

        if (thisParticle.FindProperty("ShapeModule.radius").floatValue >= MAX_RADIUS)

        {

            while (thisParticle.FindProperty("ShapeModule.radius").floatValue > MIN_RADIUS)

            {

                //grab the radius value and subtract it

                thisParticle.FindProperty("ShapeModule.radius").floatValue -= Time.deltaTime * transitionSpeed;

                thisParticle.ApplyModifiedProperties(); // This is used to apply the new radius value

                yield return null;

            }

        }

        //This code will make radius larger if radius is already at its minimum

        else if (thisParticle.FindProperty("ShapeModule.radius").floatValue <= MIN_RADIUS)

        {

            while (thisParticle.FindProperty("ShapeModule.radius").floatValue < MAX_RADIUS)

            {

                //grab the radius variable and increase it

                thisParticle.FindProperty("ShapeModule.radius").floatValue += Time.deltaTime * transitionSpeed;

                thisParticle.ApplyModifiedProperties(); // Apply new radius value

                yield return null;

            }

        }

        isChanging = false; // set to false so user can input again.

        yield return null;

    }

    IEnumerator ChangeAngle()

    {

        isChanging = true;

        //This code will make the angle smaller if the angle is at its maximum already

        if (thisParticle.FindProperty("ShapeModule.angle").floatValue >= MAX_ANGLE)

        {

            while (thisParticle.FindProperty("ShapeModule.angle").floatValue > MIN_ANGLE)

            {

                //grab angle value and subtract it

                thisParticle.FindProperty("ShapeModule.angle").floatValue -= Time.deltaTime * (transitionSpeed * 2);

                thisParticle.ApplyModifiedProperties(); // apply new value to angle

                yield return null;

            }

        }

        //This code will make angle larger if angle is already at its minimum

        else if (thisParticle.FindProperty("ShapeModule.angle").floatValue <= MIN_ANGLE)

        {

            while (thisParticle.FindProperty("ShapeModule.angle").floatValue < MAX_ANGLE)

            {

                // grab angle value and increase it

                thisParticle.FindProperty("ShapeModule.angle").floatValue += Time.deltaTime * (transitionSpeed * 2);

                thisParticle.ApplyModifiedProperties(); // apply new value to angle

                yield return null;

            }

        }

        isChanging = false;

        yield return null;

    }

}