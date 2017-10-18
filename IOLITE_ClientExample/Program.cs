using System;
using IOLITE_ClientExample.LEIServiceReference;


namespace IOLITE_ClientExample
{
    class Program
    {
        static void Main(string[] args)
        {

            FindLeiInfo(ItemChoiceType.cmp_name, @"RCB BANK LTD");

            FindLeiInfo(ItemChoiceType.lei, @"213800VDYEGJETJ1QL56");

            FindLeiInfo(ItemChoiceType.reg_num, @"1027800000250");

        }

        static void FindLeiInfo(ItemChoiceType itemElementName, string item)
        {
            using (var svc = new getLeiCodeInfo_pttClient())
            {
                var tp =      itemElementName == ItemChoiceType.cmp_name ? "имени организации" : 
                             (itemElementName == ItemChoiceType.lei      ? "коду LEI" : 
                             (itemElementName == ItemChoiceType.reg_num  ? "рег.номеру" : ""));

                Console.WriteLine($"Поиск информации по {tp} \"{item}\"");
                var resp = svc.getLeiCodeInfo(new GetLeiCodeInfoRequest
                {
                    ItemElementName = itemElementName,
                    Item = item
                });

                Console.WriteLine($"Найдено: {resp.LEIData.LEIHeader.RecordCount}");

                if (int.Parse(resp.LEIData.LEIHeader.RecordCount) > 0)
                {
                    var LEIRecord = resp.LEIData.LEIRecords.LEIRecord[0];

                    Console.WriteLine($"Наименование: {LEIRecord.Entity.LegalName.Value}");
                    Console.WriteLine($"ОГРН: {LEIRecord.Entity.RegistrationAuthority.RegistrationAuthorityEntityID}");
                    Console.WriteLine($"LEI: {LEIRecord.LEI} ({LEIRecord.Registration.RegistrationStatus})");
                    Console.WriteLine($"Страна: {LEIRecord.Entity.LegalJurisdiction}");
                    Console.WriteLine($"Адрес: {LEIRecord.Entity.LegalAddress.MailRouting}");

                }

            }
            Console.WriteLine();
            Console.ReadKey();
            Console.WriteLine();

        }

    }

}
