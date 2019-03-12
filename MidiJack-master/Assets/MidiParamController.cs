using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;


public class MidiParamController : MonoBehaviour
{
    public ParticleSystem midiParticleSystem;
    public MidiMapping midiMappingScript;
    public GameObject Logic;

    public int mappedNum;

    public int midiStartSizeNum;
    public float midiStartSize;
    public float midiStartSizePrev;
    public int midiStartSpeedNum;
    public float midiStartSpeed;
    public float midiStartSpeedPrev;
    public int midiEmissionRateNum;
    public float midiEmissionRate;
    public float midiEmissionRatePrev;
    public int midiGravityModifierNum;
    public float midiGravityModifier;
    public float midiGravityModifierPrev;
    public int midiStartLifetimeNum;
    public float midiStartLifetime;
    public float midiStartLifetimePrev;
    public int midiForceOverLifetimeNum;
    public float midiForceOverLifetime;
    public float midiForceOverLifetimePrev;
    public int midiDampenNum;
    public float midiDampen;
    public float midiDampenPrev;
    public int midiLifetimeNum;
    public float midiLifetime;
    public float midiLifetimePrev;
    public int midiShapeAngleNum;
    public float midiShapeAngle;
    public float midiShapeAnglePrev;
    public float midiSimulationSpeed;
    public float midiNoiseStrength;
    public float midiNoiseFreq;
    public float midiNoiseSizeAmount;
    public float midiTrailWidthOverTrail;

    public float particleSystemRotationSpeedX;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        midiParticleSystem.transform.Rotate(Vector3.up, particleSystemRotationSpeedX * Time.deltaTime);

    }



    public void UniController(int contNum, float contVal)
    {

        //Debug.Log("UniController is firing with " + contNum);

        //LOOK UP WHAT IS CURRENTLY MAPPED TO THAT MIDI CHANNEL
        Debug.Log("midimap converts " + contNum + " to " + midiMappingScript.particleParams[contNum] + " with value "+ contVal);
        mappedNum = midiMappingScript.particleParams[contNum];




        switch (mappedNum)

        {
            
        case 32:
                ParticleColorA(contVal);
        break;
        case 31:
                ParticleColorB(contVal);
        break;
        case 30:
                ParticleColorG(contVal);
        break;
        case 29:
                ParticleColorR(contVal);

                break;
        case 28:
                
        break; 
        case 27:
                
        break; 
        case 26:
                
                break;
            case 25:
                
                break;
            case 24:
                
                break;
            case 23:
                
                break;
            case 22:
                
                break;
            case 21:
                RotateSystemX(contVal);
                break;
            case 20:
                RendererLengthScale(contVal);
                break;
            case 19:
                RendererSpeedScale(contVal);
                break;
            case 18:
                TrailsRibbonCount(contVal);
                break;
            case 17:
                TrailsWidthOverTrail(contVal);
                break;
            case 16:
                
                NoiseSizeAmount(contVal);
                break;
            case 15:
                NoiseStrength(contVal);
                break;
            case 14:
                
                NoiseFreq(contVal);
                break;
            case 13:
                
                ForceOverLifeTimez(contVal);
                break;
            case 12:
                
                ForceOverLifeTimey(contVal);
                break;
            case 11:
                
                ForceOverLifeTimex(contVal);
                break;
            case 10:
                
                Dampen(contVal);
                break;
            case 9:
                
                ShapeRadius(contVal);
                break;
            case 8:
                
                
                ShapeAngle(contVal);
                break;
            case 7:
                
                EmissionRate(contVal);
                break;
            case 6:

                SimSpeed(contVal);
                break;
            case 5:
                
                GravityModifier(contVal);
                break;
            case 4:
                StartRotation(contVal);
                break;
            case 3:
                StartSize(contVal);
                break;
            case 2:
                StartSpeed(contVal);
                break;
            case 1:
                StartLifeTime(contVal);
                break;
            default:
                print("unassigned");
                break;
        }

        void StartLifeTime(float passedMidiValue)
        {
            midiLifetime = Map(0, 15, 0, 1, passedMidiValue);
            midiParticleSystem.startLifetime = midiLifetime;
            string parameterName = "StartLifeTime";
            ReadOut(midiLifetime, parameterName);
        }
        void StartSpeed(float passedMidiValue)
        {
            midiStartSpeed = Map(-500, 500, 0, 1, passedMidiValue);
            midiParticleSystem.startSpeed = midiStartSpeed;
            string parameterName = "StartSpeed";
            ReadOut(midiStartSpeed, parameterName);
        }
        void SimSpeed(float passedMidiValue)
        {
            var a = Map(0, 7, 0, 1, passedMidiValue);
            midiParticleSystem.playbackSpeed = a;
            midiSimulationSpeed = a;
            string parameterName = "SimSPeed";
            ReadOut(a, parameterName);
        }

        void StartSize(float passedMidiValue)
        {
            midiStartSize = Map(0, 300, 0, 1, passedMidiValue);
            midiParticleSystem.startSize = midiStartSize;
            string parameterName = "StartSize";
            ReadOut(midiStartSize, parameterName);
        }

        void EmissionRate(float passedMidiValue)
        {
            midiEmissionRate = Map(0, 500, 0, 1, passedMidiValue);
            midiParticleSystem.emissionRate = midiEmissionRate;
            string parameterName = "EmissionRate";
            ReadOut(midiEmissionRate, parameterName);
        }
        
        void GravityModifier(float passedMidiValue)
        {
            midiGravityModifier = Map(0, 500, 0, 1, passedMidiValue);
            midiParticleSystem.gravityModifier = midiGravityModifier;
            string parameterName = "GravityModifier";
            ReadOut(midiGravityModifier, parameterName);
        }
        void Dampen(float passedMidiValue)
        {
            midiDampen = Map(0, 1, 0, 1, passedMidiValue);
            var limitVelocityModule = midiParticleSystem.limitVelocityOverLifetime;
            limitVelocityModule.enabled = true;
            limitVelocityModule.dampen = midiDampen;
            string parameterName = "Dampen";
            ReadOut(midiDampen, parameterName);
        }
        
        
        void ShapeAngle(float passedMidiValue)
        {
            midiShapeAngle = Map(0, 90, 0, 1, passedMidiValue);
            var limitVelocityModule = midiParticleSystem.shape;
            // Enable it and set a value
            //limitVelocityModule.enabled = true;
            limitVelocityModule.angle = midiShapeAngle;
            string parameterName = "ShapeAngle";
            ReadOut(midiShapeAngle, parameterName);
        }
        void ForceOverLifeTimex(float passedMidiValue)
        {
            //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
            midiForceOverLifetime = Map(0, 500, 0, 1, passedMidiValue);
            var forceModule = midiParticleSystem.forceOverLifetime;
            // Enable it and set a value
            //forceModule.enabled = true;
            forceModule.x = new ParticleSystem.MinMaxCurve(midiForceOverLifetime);
            string parameterName = "ForceOverLifeTimeX";
            ReadOut(midiForceOverLifetime, parameterName);
        }
        void ForceOverLifeTimey(float passedMidiValue)
        {
            //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
            midiForceOverLifetime = Map(0, 500, 0, 1, passedMidiValue);
            var forceModule = midiParticleSystem.forceOverLifetime;
            // Enable it and set a value
            //forceModule.enabled = true;
            forceModule.y = new ParticleSystem.MinMaxCurve(midiForceOverLifetime);
            string parameterName = "ForceOverLifeTimeY";
            ReadOut(midiForceOverLifetime, parameterName);
        }
        void ForceOverLifeTimez(float passedMidiValue)
        {
            //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
            midiForceOverLifetime = Map(0, 500, 0, 1, passedMidiValue);
            var forceModule = midiParticleSystem.forceOverLifetime;
            // Enable it and set a value
            //forceModule.enabled = true;
            forceModule.z = new ParticleSystem.MinMaxCurve(midiForceOverLifetime);
            string parameterName = "ForceOverLifeTimeZ";
            ReadOut(midiForceOverLifetime, parameterName);
        }

        void NoiseFreq(float passedMidiValue)
        {
            //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
            midiNoiseFreq = Map(0, 6, 0, 1, passedMidiValue);
            var a = midiParticleSystem.noise;
            // Enable it and set a value
            //forceModule.enabled = true;
            a.frequency = midiNoiseFreq;
            string parameterName = "NoiseFreq";
            ReadOut(midiNoiseFreq, parameterName);

        }
        
        void NoiseStrength(float passedMidiValue)
        {
            //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
            midiNoiseStrength = Map(0, 6, 0, 1, passedMidiValue);
            var a = midiParticleSystem.noise;
            // Enable it and set a value
            //forceModule.enabled = true;
            a.strength = midiNoiseStrength;
            string parameterName = "NoiseStrength";
            ReadOut(midiNoiseStrength, parameterName);
        }
        
        void NoiseSizeAmount(float passedMidiValue)
        {
            //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
            midiNoiseSizeAmount = Map(0, 6, 0, 1, passedMidiValue);
            var a = midiParticleSystem.noise;
            // Enable it and set a value
            //forceModule.enabled = true;
            a.sizeAmount = midiNoiseSizeAmount;
            string parameterName = "NoiseSizeAmount";
            ReadOut(midiNoiseSizeAmount, parameterName);
        }
        
        void TrailsWidthOverTrail(float passedMidiValue)
        {
            //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
            midiTrailWidthOverTrail = Map(0, 100, 0, 1, passedMidiValue);
            var a = midiParticleSystem.trails;
            a.widthOverTrail = new ParticleSystem.MinMaxCurve(midiTrailWidthOverTrail);
            string parameterName = "TrailWidthOverTrails";
            ReadOut(midiTrailWidthOverTrail, parameterName);
        }
        void TrailsRibbonCount(float passedMidiValue)
        {
            var b = Map(0, 80, 0, 1, passedMidiValue);
            var a = midiParticleSystem.trails;
            int iValue = (int)b; // convert the float to an int 
            a.ribbonCount = iValue;
            string parameterName = "TrailRibbonCOunt";
            ReadOut(b, parameterName);
        }
        void StartRotation(float passedMidiValue)
        {
            //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
            var tempMidiVal = Map(0, 90, 0, 1, passedMidiValue);
            var a = midiParticleSystem.main;
            a.startRotation = new ParticleSystem.MinMaxCurve(tempMidiVal);
            string parameterName = "Start ROtation";
            ReadOut(tempMidiVal, parameterName);
        }
        void ShapeRadius(float passedMidiValue)
        {
            //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
            var tempMidiVal = Map(0, 90, 0, 1, passedMidiValue);
            var a = midiParticleSystem.shape;
            a.radius = tempMidiVal;
            string parameterName = "ShapeRadius";
            ReadOut(tempMidiVal, parameterName);
        }
        void RendererLengthScale(float passedMidiValue)
        {
            //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
            var tempMidiVal = Map(0, 10, 0, 1, passedMidiValue);
            //var a = midiParticleSystem.Renderer;
            var a = midiParticleSystem.GetComponent<ParticleSystemRenderer>();
            a.lengthScale = tempMidiVal;
            string parameterName = "RenderLengthScale";
            ReadOut(tempMidiVal, parameterName);
        }
        void RendererSpeedScale(float passedMidiValue)
        {
            //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
            var tempMidiVal = Map(0, 10, 0, 1, passedMidiValue);
            //var a = midiParticleSystem.Renderer;
            var a = midiParticleSystem.GetComponent<ParticleSystemRenderer>();
            a.velocityScale = tempMidiVal;
            string parameterName = "RendSpeedScale";
            ReadOut(tempMidiVal, parameterName);
        }
        void ParticleColorR(float passedMidiValue)
        {
            //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
            var tempMidiVal = Map(0, 255, 0, 1, passedMidiValue);
            //var a = midiParticleSystem.Renderer;
            var a = midiParticleSystem.startColor;
            var tempColor = new Color(tempMidiVal, 0, 1, .5f);
            a = tempColor;
            string parameterName = "Particle COlor R";
            ReadOut(tempMidiVal, parameterName);


        }
        void ParticleColorG(float passedMidiValue)
        {
            //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
            var tempMidiVal = Map(0, 1, 0, 1, passedMidiValue);
            //var a = midiParticleSystem.Renderer;
            var a = midiParticleSystem.startColor;
            a = Color.green;
            string parameterName = "Particle COlor G";
            ReadOut(tempMidiVal, parameterName);
        }
        void ParticleColorB(float passedMidiValue)
        {
            //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
            var tempMidiVal = Map(0, 1, 0, 1, passedMidiValue);
            //var a = midiParticleSystem.Renderer;
            var a = midiParticleSystem.startColor;
            a.b = tempMidiVal;
            string parameterName = "Particle COlor B";
            ReadOut(tempMidiVal, parameterName);
        }
        void ParticleColorA(float passedMidiValue)
        {
            
            //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
            var tempMidiVal = Map(0, 1, 0, 1, passedMidiValue);
            //var a = midiParticleSystem.Renderer;
            var a = midiParticleSystem.startColor;
            a.a = tempMidiVal;
            string parameterName = "Particle COlor A";
            ReadOut(tempMidiVal, parameterName);
        }
        //transform.Rotate(Vector3.up, speed * Time.deltaTime);
        void RotateSystemX(float passedMidiValue)
        {
            //version for setting an attribute that is a STRUCT https://blogs.unity3d.com/2016/04/20/particle-system-modules-faq/
            var tempMidiVal = Map(-100, 100, 0, 1, passedMidiValue);
            particleSystemRotationSpeedX = tempMidiVal;
            //midiParticleSystem.transform.Rotate(Vector3.up, tempMidiVal * Time.deltaTime);
            string parameterName = "RotateSystemX";
            ReadOut(tempMidiVal, parameterName);
        }

        void ReadOut(float tempMidiVal, string parameterName)
        {
            Debug.Log("parameter  " + parameterName + " with value " + tempMidiVal);

        }

        // used to normalize any value range (new min, new max, old min, old max, oldValue)
        float Map(float from, float to, float from2, float to2, float value)
        {
            if (value <= from2)
            {
                return from;
            }
            else if (value >= to2)
            {
                return to;
            }
            else
            {
                return (to - from) * ((value - from2) / (to2 - from2)) + from;
            }
        }
    }
}