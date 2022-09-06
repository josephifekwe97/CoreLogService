using System;
namespace Core.LogService.Data
{
    public class FileDto
    {
        public FileDto()
        {
        }
    }

    public class nip_accountblock_logs
    {
        public string SessionID { get; set; }
        public string DestinationInstitutionCode { get; set; }
        public string ChannelCode { get; set; }
        public string ReferenceCode { get; set; }
        public string TargetAccountName { get; set; }
        public string TargetBankVerificationNumber { get; set; }
        public string TargetAccountNumber { get; set; }
        public string ReasonCode { get; set; }
        public string Narration { get; set; }
    }
}
