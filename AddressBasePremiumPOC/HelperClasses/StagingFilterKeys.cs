using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TQ.Geocoding.DataLoad.HelperClasses
{
    public static class StagingFilterKeys
    {
        public static List<string> StagingUprns;
        public static List<string> StagingUsrns;
        public static List<string> StagingLpiKeys;
        public static List<string> StagingXrefKeys;
        public static List<string> StagingClassKeys;
        public static List<string> StagingUdprns;
        public static List<string> StagingOrgKeys;

        static StagingFilterKeys()
        {
            StagingUprns = new List<string>();
            StagingUsrns = new List<string>();
            StagingLpiKeys = new List<string>();
            StagingXrefKeys = new List<string>();
            StagingClassKeys = new List<string>();
            StagingUdprns = new List<string>();
            StagingOrgKeys = new List<string>();
        }
    }
}
