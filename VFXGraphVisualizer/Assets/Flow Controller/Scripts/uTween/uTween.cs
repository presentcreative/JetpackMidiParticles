/* Copyright Kupio Limited SC426881. All rights reserved. Source not for distribution. */

namespace com.kupio.uTween
{
    using UnityEngine;

    public class UTween
    {

        /**
         * Simple linear tween.
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float Linear(float t, float b, float c, float d)
        {
            return b + c * Mathf.Min(1, t / d);
        }

        public static float Linear01(float t)
        {
            return Mathf.Min(1, t); /* Technically returns t, but we include it for completeness */
        }

        /**
         * Eases in and out of the animation
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float EaseInOutCubic(float t, float b, float c, float d)
        {
            t = Mathf.Min(d, t);
            var ts = (t /= d) * t;
            var tc = ts * t;
            return b + c * (-2 * tc + 3 * ts);
        }
        public static float EaseInOutCubic01(float t)
        {
            t = Mathf.Min(1, t);
            var ts = t * t;
            var tc = ts * t;
            return -2 * tc + 3 * ts;
        }

        /**
         * Eases softly in and out of the animation
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float EaseInOutQuintic(float t, float b, float c, float d)
        {
            t = Mathf.Min(d, t);
            var ts = (t /= d) * t;
            var tc = ts * t;
            return b + c * (6 * tc * ts + -15 * ts * ts + 10 * tc);
        }
        public static float EaseInOutQuintic01(float t)
        {
            t = Mathf.Min(1, t);
            var ts = t * t;
            var tc = ts * t;
            return 6 * tc * ts + -15 * ts * ts + 10 * tc;
        }

        /**
         * Eases very softly into the animation
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float EaseInQuintic(float t, float b, float c, float d)
        {
            t = Mathf.Min(d, t);
            var ts = (t /= d) * t;
            var tc = ts * t;
            return b + c * (tc * ts);
        }
        public static float EaseInQuintic01(float t)
        {
            t = Mathf.Min(1, t);
            var ts = t * t;
            var tc = ts * t;
            return tc * ts;
        }

        /**
         * Eases softly into the animation
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float EaseInQuartic(float t, float b, float c, float d)
        {
            t = Mathf.Min(d, t);
            var ts = (t /= d) * t;
            return b + c * (ts * ts);
        }
        public static float EaseInQuartic01(float t)
        {
            t = Mathf.Min(1, t);
            var ts = t * t;
            return ts * ts;
        }

        /**
         * Eases into the animation
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float EaseInCubic(float t, float b, float c, float d)
        {
            t = Mathf.Min(d, t);
            var tc = (t /= d) * t * t;
            return b + c * (tc);
        }
        public static float EaseInCubic01(float t)
        {
            t = Mathf.Min(1, t);
            return t * t * t;
        }

        /**
         * Eases quickly into the animation
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float EaseInQuadratic(float t, float b, float c, float d)
        {
            t = Mathf.Min(d, t);
            return b + c * (t * t / d);
        }
        public static float EaseInQuadratic01(float t)
        {
            t = Mathf.Min(1, t);
            return t * t;
        }

        /**
         * Eases very softly out of the animation
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float EaseOutQuintic(float t, float b, float c, float d)
        {
            t = Mathf.Min(d, t);
            var ts = (t /= d) * t;
            var tc = ts * t;
            return b + c * (tc * ts + -5 * ts * ts + 10 * tc + -10 * ts + 5 * t);
        }
        public static float EaseOutQuintic01(float t)
        {
            t = Mathf.Min(1, t);
            var ts = t * t;
            var tc = ts * t;
            return tc * ts + -5 * ts * ts + 10 * tc + -10 * ts + 5 * t;
        }

        /**
         * Eases softly out of the animation
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float EaseOutQuartic(float t, float b, float c, float d)
        {
            t = Mathf.Min(d, t);
            var ts = (t /= d) * t;
            var tc = ts * t;
            return b + c * (-1 * ts * ts + 4 * tc + -6 * ts + 4 * t);
        }
        public static float EaseOutQuartic01(float t)
        {
            t = Mathf.Min(1, t);
            var ts = t * t;
            var tc = ts * t;
            return -1 * ts * ts + 4 * tc + -6 * ts + 4 * t;
        }

        /**
         * Eases out of the animation
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float EaseOutCubic(float t, float b, float c, float d)
        {
            t = Mathf.Min(d, t);
            var ts = (t /= d) * t;
            var tc = ts * t;
            return b + c * (tc + -3 * ts + 3 * t);
        }
        public static float EaseOutCubic01(float t)
        {
            t = Mathf.Min(1, t);
            var ts = t * t;
            var tc = ts * t;
            return tc + -3 * ts + 3 * t;
        }

        /** Opposite of easing in and out. Starts and ends linearly, but
         * comes to a pause in the middle.
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float EaseOutInCubic(float t, float b, float c, float d)
        {
            t = Mathf.Min(d, t);
            var ts = (t /= d) * t;
            var tc = ts * t;
            return b + c * (4 * tc + -6 * ts + 3 * t);
        }
        public static float EaseOutInCubic01(float t)
        {
            t = Mathf.Min(1, t);
            var ts = t * t;
            var tc = ts * t;
            return 4 * tc + -6 * ts + 3 * t;
        }

        /** Moves back first before easing in.
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float BackInCubic(float t, float b, float c, float d)
        {
            t = Mathf.Min(d, t);
            var ts = (t /= d) * t;
            var tc = ts * t;
            return b + c * (4 * tc + -3 * ts);
        }
        public static float BackInCubic01(float t)
        {
            t = Mathf.Min(1, t);
            var ts = t * t;
            var tc = ts * t;
            return 4 * tc + -3 * ts;
        }

        /** Moves back first before easing in.
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float BackInQuartic(float t, float b, float c, float d)
        {
            t = Mathf.Min(d, t);
            var ts = (t /= d) * t;
            var tc = ts * t;
            return b + c * (2 * ts * ts + 2 * tc + -3 * ts);
        }
        public static float BackInQuartic01(float t)
        {
            t = Mathf.Min(1, t);
            var ts = t * t;
            var tc = ts * t;
            return 2 * ts * ts + 2 * tc + -3 * ts;
        }

        /** Overshoots, then eases back
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float OutBackCubic(float t, float b, float c, float d)
        {
            t = Mathf.Min(d, t);
            var ts = (t /= d) * t;
            var tc = ts * t;
            return b + c * (4 * tc + -9 * ts + 6 * t);
        }
        public static float OutBackCubic01(float t)
        {
            t = Mathf.Min(1, t);
            var ts = t * t;
            var tc = ts * t;
            return 4 * tc + -9 * ts + 6 * t;
        }

        /** Overshoots, then eases back
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float OutBackQuartic(float t, float b, float c, float d)
        {
            t = Mathf.Min(d, t);
            var ts = (t /= d) * t;
            var tc = ts * t;
            return b + c * (-2 * ts * ts + 10 * tc + -15 * ts + 8 * t);
        }
        public static float OutBackQuartic01(float t)
        {
            t = Mathf.Min(1, t);
            var ts = t * t;
            var tc = ts * t;
            return -2 * ts * ts + 10 * tc + -15 * ts + 8 * t;
        }

        /** Bounces around the target point, then settles.
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float BounceOut(float t, float b, float c, float d)
        {
            t = Mathf.Min(d, t);
            var ts = (t /= d) * t;
            var tc = ts * t;
            return b + c * (33 * tc * ts + -106 * ts * ts + 126 * tc + -67 * ts + 15 * t);
        }
        public static float BounceOut01(float t)
        {
            t = Mathf.Min(1, t);
            var ts = t * t;
            var tc = ts * t;
            return 33 * tc * ts + -106 * ts * ts + 126 * tc + -67 * ts + 15 * t;
        }

        /** Bounces around the start point, then moves quickly to the target.
         * @param  {Number} t Current time passed since the beginning of the animation. Must be >=0.
         * Will be clamped to the duration.
         * @param  {Number} b The start value of the property being tweened
         * @param  {Number} c The desired delta. E.g. if b = 10, and you want to tween it to 30, c
         * should be 20
         * @param  {Number} d The duration in the same units as t.
         * @return {Number} Current value at the given time.
         */
        public static float BounceIn(float t, float b, float c, float d)
        {
            t = Mathf.Min(d, t);
            var ts = (t /= d) * t;
            var tc = ts * t;
            return b + c * (33 * tc * ts + -59 * ts * ts + 32 * tc + -5 * ts);
        }
        public static float BounceIn01(float t)
        {
            t = Mathf.Min(1, t);
            var ts = t * t;
            var tc = ts * t;
            return 33 * tc * ts + -59 * ts * ts + 32 * tc + -5 * ts;
        }
    }
}
