using CovidonusApi.Models;
using System;

namespace CovidonusApi.Helpers
{
    public static class AuditorHelper
    {
        public static void MapCreated<T>(T obj, string userName = null) where T : Auditor
        {
            if (obj != null)
            {
                obj.Created = DateTime.Now;
                obj.CreatedBy = userName;
                obj.IsActive = true;
            }
        }

        public static void MapModified<T>(T obj, string userName = null) where T : Auditor
        {
            if (obj != null)
            {
                obj.Modified = DateTime.Now;
                obj.ModifiedBy = userName;
            }
        }
    }
}