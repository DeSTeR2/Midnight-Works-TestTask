using System;

namespace Animation.Ease
{
    public enum EasingType
    {
        Linear,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InSine,
        OutSine,
        InOutSine,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InElastic,
        OutElastic,
        InOutElastic,
        InBack,
        OutBack,
        InOutBack,
        InBounce,
        OutBounce,
        InOutBounce
    }

    public static class EasingConverter
    {
        public static Func<float, float> GetEasingFunction(EasingType easingType)
        {
            switch (easingType)
            {
                case EasingType.Linear:
                    return EasingFunctions.Linear;
                case EasingType.InQuad:
                    return EasingFunctions.InQuad;
                case EasingType.OutQuad:
                    return EasingFunctions.OutQuad;
                case EasingType.InOutQuad:
                    return EasingFunctions.InOutQuad;
                case EasingType.InCubic:
                    return EasingFunctions.InCubic;
                case EasingType.OutCubic:
                    return EasingFunctions.OutCubic;
                case EasingType.InOutCubic:
                    return EasingFunctions.InOutCubic;
                case EasingType.InQuart:
                    return EasingFunctions.InQuart;
                case EasingType.OutQuart:
                    return EasingFunctions.OutQuart;
                case EasingType.InOutQuart:
                    return EasingFunctions.InOutQuart;
                case EasingType.InQuint:
                    return EasingFunctions.InQuint;
                case EasingType.OutQuint:
                    return EasingFunctions.OutQuint;
                case EasingType.InOutQuint:
                    return EasingFunctions.InOutQuint;
                case EasingType.InSine:
                    return EasingFunctions.InSine;
                case EasingType.OutSine:
                    return EasingFunctions.OutSine;
                case EasingType.InOutSine:
                    return EasingFunctions.InOutSine;
                case EasingType.InExpo:
                    return EasingFunctions.InExpo;
                case EasingType.OutExpo:
                    return EasingFunctions.OutExpo;
                case EasingType.InOutExpo:
                    return EasingFunctions.InOutExpo;
                case EasingType.InCirc:
                    return EasingFunctions.InCirc;
                case EasingType.OutCirc:
                    return EasingFunctions.OutCirc;
                case EasingType.InOutCirc:
                    return EasingFunctions.InOutCirc;
                case EasingType.InElastic:
                    return EasingFunctions.InElastic;
                case EasingType.OutElastic:
                    return EasingFunctions.OutElastic;
                case EasingType.InOutElastic:
                    return EasingFunctions.InOutElastic;
                case EasingType.InBack:
                    return EasingFunctions.InBack;
                case EasingType.OutBack:
                    return EasingFunctions.OutBack;
                case EasingType.InOutBack:
                    return EasingFunctions.InOutBack;
                case EasingType.InBounce:
                    return EasingFunctions.InBounce;
                case EasingType.OutBounce:
                    return EasingFunctions.OutBounce;
                case EasingType.InOutBounce:
                    return EasingFunctions.InOutBounce;
                default:
                    throw new ArgumentException($"Unsupported easing type: {easingType}");
            }
        }
    }
}
