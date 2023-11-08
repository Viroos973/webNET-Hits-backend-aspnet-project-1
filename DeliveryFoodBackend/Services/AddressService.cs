using DeliveryFoodBackend.Data;
using DeliveryFoodBackend.Data.Models.Enums;
using DeliveryFoodBackend.DTO;
using DeliveryFoodBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DeliveryFoodBackend.Service
{
    public class AddressService : IAddressService
    {
        private readonly AppDbContext _context;

        public AddressService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SearchAddressModel>> AddressChain(Guid objectGuid)
        {
            List<SearchAddressModel> addressChain = new List<SearchAddressModel>();

            var addressDto = _context.AsHouses.Where(x => x.Objectguid == objectGuid).Join(_context.AsAdmHierarchies, house => house.Objectid, adm => adm.Objectid,
                    (house, adm) => adm.Path).FirstOrDefault();

            if (addressDto == null)
            {
                addressDto = _context.AsAddrObjs.Where(x => x.Objectguid == objectGuid).Join(_context.AsAdmHierarchies, house => house.Objectid, adm => adm.Objectid,
                    (house, adm) => adm.Path).FirstOrDefault();

                if (addressDto == null)
                {
                    throw new BadHttpRequestException(message: $"Not found object with ObjectGuid={objectGuid}");
                }
            }

            var path = addressDto.Split(".");

            foreach (var pathObj in path)
            {
                var addressObj = _context.AsAddrObjs.Where(x => x.Objectid.ToString() == pathObj).Select(x => new SearchAddressModel
                {
                    ObjectId = x.Objectid,
                    ObgectGuid = x.Objectguid,
                    Text = $"{x.Typename} {x.Name}",
                    ObjectLevel = (GarAddressLevel)(int.Parse(x.Level) - 1),
                    ObjectLevelText = GetLevelText(x.Level)
                }).FirstOrDefault();

                if (addressObj == null)
                {
                    addressObj = _context.AsHouses.Where(x => x.Objectid.ToString() == pathObj).Select(x => new SearchAddressModel
                    {
                        ObjectId = x.Objectid,
                        ObgectGuid = x.Objectguid,
                        Text = $"{x.Housenum}{GetHouseType(x.Addtype1)}{x.Addnum1}{GetHouseType(x.Addtype2)}{x.Addnum2}",
                        ObjectLevel = (GarAddressLevel)9,
                        ObjectLevelText = "Здание (сооружение)"
                    }).FirstOrDefault();
                }
                addressChain.Add(addressObj);
            }

            return addressChain;
        }

        public async Task<List<SearchAddressModel>> AddressSearch(int? parentObj, string? query)
        {
            if (parentObj == null)
            {
                parentObj = 0;
            }

            if (query == null)
            {
                query = "";
            }

            var adm = await _context.AsAdmHierarchies.Where(x => x.Parentobjid == parentObj).Select(x => x.Objectid).ToListAsync();

            if (adm != null)
            {
                var houseList = _context.AsHouses.Where(x => adm.Contains(x.Objectid) && x.Isactual == 1).Select(x => new SearchAddressModel
                {
                    ObjectId = x.Objectid,
                    ObgectGuid = x.Objectguid,
                    Text = $"{x.Housenum}{GetHouseType(x.Addtype1)}{x.Addnum1}{GetHouseType(x.Addtype2)}{x.Addnum2}",
                    ObjectLevel = (GarAddressLevel)9,
                    ObjectLevelText = "Здание (сооружение)"
                }).ToList();

                var addrList = _context.AsAddrObjs.Where(x => adm.Contains(x.Objectid) && x.Isactual == 1).Select(x => new SearchAddressModel
                {
                    ObjectId = x.Objectid,
                    ObgectGuid = x.Objectguid,
                    Text = $"{x.Typename} {x.Name}",
                    ObjectLevel = (GarAddressLevel)(int.Parse(x.Level) - 1),
                    ObjectLevelText = GetLevelText(x.Level)
                }).ToList();

                return addrList.Concat(houseList).Where(x => Regex.IsMatch(x.Text ,query)).Take(10).ToList();
            }

            return null;
        }

        private static string GetHouseType(int? type)
        {
            switch (type)
            {
                case 1:
                    return " влд. ";
                case 2:
                    return " д. ";
                case 3:
                    return " двлд. ";
                case 4:
                    return " г-ж ";
                case 5:
                    return " зд. ";
                default:
                    return "";
            }
        }

        private static string GetLevelText(string level)
        {
            switch (level)
            {
                case "1":
                    return "Субъект РФ";
                case "2":
                    return "Административный район";
                case "3":
                    return "Муниципальный район";
                case "4":
                    return "Поселение";
                case "5":
                    return "Город";
                case "6":
                    return "Населённый пункт";
                case "7":
                    return "Элемент планировочной структуры";
                case "8":
                    return "Элемент улично-дорожной сети";
                case "9":
                    return "Земельный участок";
                case "11":
                    return "Помещение в пределах здания";
                case "12":
                    return "Помещение в пределах помещения";
                case "13":
                    return "Автономная область";
                case "14":
                    return "Внутригородской";
                case "15":
                    return "Прочие территории";
                case "16":
                    return "Элемент прочих территорий";
                case "17":
                    return "Машино-место";
                default:
                    return "";
            }
        }
    }
}
