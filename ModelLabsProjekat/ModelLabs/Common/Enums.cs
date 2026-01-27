using System;

namespace FTN.Common
{
    public enum SealConditionKind : short
    {
        broken = 0,
        locked = 1,
        missing = 2,
        open = 3,
        other = 4
    }

    public enum SealKind : short
    {
        lead = 0,
        @lock = 1,
        steel = 2,
        other = 3
    }

    public enum CorporateStandardKind : short
    {
        experimental = 0,
        other = 1,
        standard = 2,
        underEvaluation = 3
    }

    public enum AssetModelUsageKind : short
    {
        customerSubstation = 0,
        distributionOverhead = 1,
        distributionUnderground = 2,
        other = 3,
        streetlight = 4,
        substation = 5,
        transmission = 6,
        unknown = 7
    }
}
