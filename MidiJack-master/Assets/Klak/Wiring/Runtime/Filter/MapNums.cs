//
// Klak - Utilities for creative coding with Unity
//
// Copyright (C) 2016 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using UnityEngine;
using Klak.Math;

namespace Klak.Wiring
{
    [AddComponentMenu("Klak/Wiring/Filter/MapNums")]
    public class MapNums : NodeBase
    {
        #region Editable properties

        [SerializeField]
        AnimationCurve _responseCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [SerializeField]
        FloatInterpolator.Config _interpolator = FloatInterpolator.Config.Direct;

        [SerializeField]
        float _amplitude = 1.0f;

        [SerializeField]
        float _bias = 0.0f;

        [SerializeField]
        float _newMin = 0.0f;

        [SerializeField]
        float _newMax = 100.0f;

        [SerializeField]
        float _oldMin = 0.0f;

        [SerializeField]
        float _oldMax = 1.0f;

        [SerializeField]
        float _startNum;

        [SerializeField]
        float _endNum;

        #endregion

        #region Node I/O

        [Inlet]
        public float input
        {
            set
            {
                if (!enabled) return;

                _inputValue = value;

                if (_interpolator.enabled)
                    _floatValue.targetValue = EvalResponse();
                else
                    _outputEvent.Invoke(EvalResponse());
            }
        }

        [SerializeField, Outlet]
        FloatEvent _outputEvent = new FloatEvent();

        #endregion

        #region Private members
        
        float _inputValue;
        FloatInterpolator _floatValue;

        float EvalResponse()
        {
            return _responseCurve.Evaluate(_inputValue) * _amplitude + _bias;
        }

        #endregion

        #region MonoBehaviour functions

        void Start()
        {
            _floatValue = new FloatInterpolator(EvalResponse(), _interpolator);
        }

        void Update()
        {
            if (_interpolator.enabled)
                _startNum = _inputValue;
                Map(_oldMin, _oldMax, _newMin, _newMax,_inputValue);
                _outputEvent.Invoke(_floatValue.Step());
                _endNum = _floatValue.Step();

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
        #endregion
    }
}
