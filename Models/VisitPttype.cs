using System;
using System.Collections.Generic;

namespace HosAuthenAPI.Models;

public partial class VisitPttype
{
    public string Vn { get; set; } = null!;

    public string Pttype { get; set; } = null!;

    public string? Staff { get; set; }

    public double? RcptAmount { get; set; }

    public double? DebtAmount { get; set; }

    public double? DiscountAmount { get; set; }

    public DateOnly? BeginDate { get; set; }

    public DateOnly? ExpireDate { get; set; }

    public string? Hospmain { get; set; }

    public string? Hospsub { get; set; }

    public string? Pttypeno { get; set; }

    public int? PttypeNumber { get; set; }

    public int? PttypeOrder { get; set; }

    public double? DiscountPercent { get; set; }

    public int? CompanyId { get; set; }

    public int? ContractId { get; set; }

    public double? MaxDebtAmount { get; set; }

    public double? PaidAmount { get; set; }

    public string? ClaimCode { get; set; }

    public string? HosGuid { get; set; }

    public int? LimitHour { get; set; }

    public string? CheckLimitHour { get; set; }

    public string? FinanceClearOk { get; set; }

    public string? HosGuidExt { get; set; }

    public DateTime? ConfirmAndLockedDatetime { get; set; }

    public string? ConfirmAndLocked { get; set; }

    public string? ConfirmAndLockedStaff { get; set; }

    public string? NhsoGovcode { get; set; }

    public string? NhsoGovname { get; set; }

    public string? NhsoDocno { get; set; }

    public string? NhsoOwnrightPid { get; set; }

    public string? NhsoOwnrightName { get; set; }

    public DateTime? UpdateDatetime { get; set; }

    public string? EmpPrivilege { get; set; }

    public int? EmpId { get; set; }

    public string? PttypeServiceCharge { get; set; }

    public string? PttypeNote { get; set; }

    public string? AuthCode { get; set; }

    public string? RcpnoList { get; set; }

    public string? ProjectCode { get; set; }

    public string? DepcodeServiceCharge { get; set; }

    public string? RequestFunds { get; set; }

    public string? NhsoUcaeTypeCode { get; set; }
}
