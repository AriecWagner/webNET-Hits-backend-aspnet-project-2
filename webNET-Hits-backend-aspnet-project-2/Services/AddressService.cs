using System;
using System.Collections.Generic;
using System.Linq;
using webNET_Hits_backend_aspnet_project_2.Data;
using webNET_Hits_backend_aspnet_project_2.Models.DbModels;
using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using Microsoft.EntityFrameworkCore;
using webNET_Hits_backend_aspnet_project_2.Models.AnotherModels;

namespace webNET_Hits_backend_aspnet_project_2.Services
{
    public class AddressService
    {
        private readonly AddressDbContext _dbContext;
        private List<AsAddrObj> asAddrObjTable = new List<AsAddrObj>();
        private List<AsHouses> AsHousesTable = new List<AsHouses>();

        public AddressService(AddressDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Dictionary<string, string> typeMappingsObjectText = new Dictionary<string, string>
        {
            { "1", "Субъект РФ" },
            { "2", "Административный район" },
            { "3", "Муниципальный район" },
            { "4", "Сельское/городское поселение" },
            { "5", "Город" },
            { "6", "Населенный пункт" },
            { "7", "Элемент планировочной структуры" },
            { "8", "Элемент улично-дорожной сети" },
            { "9", "Земельный участок" },
            { "10", "Здание (сооружение)" },
            { "11", "Помещение" },
            { "12", "Помещения в пределах помещения" },
            { "13", "Уровень автономного округа" },
            { "14", "Уровень внутригородской территории" },
            { "15", "Уровень дополнительных территорий" },
            { "16", "Уровень объектов на дополнительных территориях" },
            { "17", "Машиноместо" }
        };

        public List<SearchAddressModelDTO>? SearchAddress(long parent, string? query)
        {
            if (query != null)
            {
                query = query.ToUpper();
            }

            List<AsAdmHierarchy> childrenElements = _dbContext.as_adm_hierarchy.Where(item => item.parentobjid == parent && item.isactive == 1).ToList();

            asAddrObjTable = _dbContext.as_addr_obj
                .Where(item => item.isactual == 1 && item.isactive == 1 && childrenElements.Select(c => c.objectid).Contains(item.objectid))
                .ToList();
            AsHousesTable = _dbContext.as_houses
                .Where(item => item.isactual == 1 && item.isactive == 1 && childrenElements.Select(c => c.objectid).Contains(item.objectid))
                .ToList();

            List<SearchAddressModelDTO> addresses = new List<SearchAddressModelDTO>();

            foreach (AsAdmHierarchy childElement in childrenElements)
            {
                AsAddrObj address = asAddrObjTable.FirstOrDefault(addr => addr.objectid == childElement.objectid);
                if (address != null)
                {
                    if (checkQuery(address.name, query) || checkQuery(address.typename, query))
                    {
                        addresses.Add(new SearchAddressModelDTO
                        {
                            ObjectId = address.objectid,
                            ObjectGuid = address.objectguid,
                            Text = address.name,
                            ObjectLevel = getObjLevel(address.objectid),
                            ObjectLevelText = getObjLevelText(address.objectid)
                        });
                    }
                }

                AsHouses house = AsHousesTable.FirstOrDefault(h => h.objectid == childElement.objectid);
                if (house != null)
                {
                    if (checkQuery(house.housenum, query))
                    {
                        addresses.Add(new SearchAddressModelDTO
                        {
                            ObjectId = house.objectid,
                            ObjectGuid = house.objectguid,
                            Text = house.housenum,
                            ObjectLevel = getObjLevel(house.objectid),
                            ObjectLevelText = getObjLevelText(house.objectid)
                        });
                    }
                }
            }
            return addresses;
        }

        public bool checkQuery(string name, string? query)
        {
            return query == null ? true : name.ToUpper().Contains(query.ToUpper());
        }

        public GarAddressLevel getObjLevel(long id)
        {
            AsHouses house = AsHousesTable.FirstOrDefault(h => h.objectid == id);
            AsAddrObj address = asAddrObjTable.FirstOrDefault(h => h.objectid == id);
            //int level = int.Parse(address.level);

            return house != null ? GarAddressLevel.Building : (GarAddressLevel)(int.Parse(address.level) - 1);
        }

        public string getObjLevelText(long id)
        {
            AsHouses house = AsHousesTable.FirstOrDefault(h => h.objectid == id);
            AsAddrObj address = asAddrObjTable.FirstOrDefault(h => h.objectid == id);

            return house != null ? "Здание (Строение)" : typeMappingsObjectText[address.level];
        }
    }
}
