using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace Mas.Infrastructure.BlazorComponents
{
    public class StoredSRData : IComparable<StoredSRData>
    {
        public string Interface { get; set; }
        public string SturdyRef { get; set; } = "";
        public string PetName { get; set; }
        public bool AutoConnect { get; set; } = false;
        public bool DefaultSelect { get; set; } = false;

        public int CompareTo(StoredSRData other)
        {
            // A null value means that this object is greater.
            if (other == null)
                return 1;
            if (PetName != null && other.PetName != null)
                return PetName.CompareTo(other.PetName);
            else
                return SturdyRef.CompareTo(other.SturdyRef);
        }

        public static string StorageKey { get; set; } = "sturdy-ref-store";

        public static async Task<List<StoredSRData>> GetAllData(ILocalStorageService service)
        {
            return await service.GetItemAsync<List<StoredSRData>>(StorageKey);
        }

        public static async Task<List<StoredSRData>> SaveNew(ILocalStorageService service, StoredSRData newData)
        {
            var all = await GetAllData(service);
            all.Add(newData);
            return await SaveAllData(service, all);
        }

        public static async Task<List<StoredSRData>> SaveAllData(ILocalStorageService service, List<StoredSRData> allData)
        {
            await service.SetItemAsync(StorageKey, allData);
            return allData;
        }
    }
}
