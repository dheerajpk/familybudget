using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyBudget.Core.Models;

namespace FamilyBudget.Core.Services
{
    public class FamilyService
    {
        private readonly SlackService _slackService;

        private readonly StorageService _storageService;

        public string FamilyCode { get; private set; }

        public string MemberId { get; private set; }

        public Family Family { get; private set; }

        public async Task<bool> IsFamilyCodeSet()
        {
            var familyCode = await _storageService.ReadFileContent("familyCode.dat").ConfigureAwait(false);

            FamilyCode = familyCode;

            MemberId = await _storageService.ReadFileContent("familyMember.dat").ConfigureAwait(false);

            return familyCode != null;
        }

        public FamilyService()
        {
            _slackService = new SlackService();

            _storageService = new StorageService();
        }

        public async Task<string> SetUpFamily(string familyName, string memberName)
        {
            Family family = new Family()
            {
                FamilyName = familyName,
                FamilyCode = Utility.KeyGenerator.GetUniqueKey(),
                ExpenseCycleStartDay = 1,
                FamilyMembers = new List<FamilyMember>() { new FamilyMember()
                {
                    IsParent = true,
                    MemberId = Guid.NewGuid().ToString(),
                    MemberName = memberName
                } }
            };

            var familySchema = new FamilySchema<Family>()
            {
                Schema = FamilySchemaConstants.Family,
                Data = family
            };

            await _slackService.Setup().ConfigureAwait(false);

            await _storageService.WriteToFile("familyCode.dat", family.FamilyCode).ConfigureAwait(false);

            return await _slackService.PostMessage(familySchema).ConfigureAwait(false);

        }

        public async Task<FamilySchema<Family>> GetFamily()
        {
            var allmessages = await _slackService.GetMessages().ConfigureAwait(false);

            var familyMessage = allmessages?.FirstOrDefault(x => x.Text != null && x.Text.Contains("\"schema\":\"Family\""));

            if (familyMessage == null) return null;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<FamilySchema<Family>>(familyMessage.Text);

            result.ExternalRefernceId = familyMessage.Ts;

            return result;
        }

        public async Task LoadFamilyDetails(string familyCode)
        {
            var familySchemaResult = await GetFamily().ConfigureAwait(false);

            var family = familySchemaResult.Data;

            if (family != null && family.FamilyCode == familyCode)
            {
                Family = family;
            }
        }

        public async Task<bool> JoinFamily(string familyCode, string memberName)
        {
            var familySchemaResult = await GetFamily().ConfigureAwait(false);

            var family = familySchemaResult.Data;

            if (family != null && family.FamilyCode == familyCode)
            {
                var externalRefernceId = familySchemaResult.ExternalRefernceId;

                family.FamilyMembers = family.FamilyMembers ?? new List<FamilyMember>();

                var familymember = family.FamilyMembers.FirstOrDefault(x => x.MemberName != null && x.MemberName.Equals(memberName, StringComparison.OrdinalIgnoreCase));

                if (familymember == null)
                {
                    family.FamilyMembers.Add(new FamilyMember()
                    {
                        IsParent = false,
                        MemberName = memberName,
                        MemberId = Guid.NewGuid().ToString()
                    });


                    var familySchema = new FamilySchema<Family>()
                    {
                        Schema = FamilySchemaConstants.Family,
                        Data = family
                    };

                    await _storageService.WriteToFile("familyCode.dat", family.FamilyCode).ConfigureAwait(false);

                    await _storageService.WriteToFile("familyMember.dat", family.FamilyMembers.Last().MemberId).ConfigureAwait(false);

                    return await _slackService.UpdateMessage(familySchema, externalRefernceId).ConfigureAwait(false);

                }

                await _storageService.WriteToFile("familyCode.dat", family.FamilyCode).ConfigureAwait(false);

                await _storageService.WriteToFile("familyMember.dat", familymember.MemberId).ConfigureAwait(false);
            }

            return false;
        }
    }
}
