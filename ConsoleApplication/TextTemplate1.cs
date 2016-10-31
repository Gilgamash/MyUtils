using System;

public partial class ImageBasicInfo
{
	#region sql
	///<summary>查询SQL</summary>
	public static string selectSql = "select IBIId,IBIImageName,IBIImageUrl,IBICopyright,IBIAuditStatus,IBIRowStatus,IBICreaterName,IBICreaterWorkNumber,IBICreateTime,IBIAuditName,IBIAuditWorkNumber,IBIAuditTime,IBIRemark,IBIImageSize,IBIImageHeight,IBIImageWidth,IBIImageType,IBICountry,IBICity,IBIExpiresDate,IBIPersonType,IBIRefuseReasion from TCInterVacationCommon.dbo.ImageBasicInfo with(nolock) where 1=1";
	#endregion
	#region 表字段
	///<summary>主键，图片Id,自增长(1,1)</summary>
	public int IBIId { get; set; }
	///<summary>图片名称</summary>
	public string IBIImageName { get; set; }
	///<summary>图片Url</summary>
	public string IBIImageUrl { get; set; }
	///<summary>图片版权(0,未购买;1已购买;2,免费使用;3不确定)</summary>
	public byte IBICopyright { get; set; }
	///<summary>使用状态</summary>
	public byte IBIAuditStatus { get; set; }
	///<summary>行状态,(无效,0;有效,1)</summary>
	public byte IBIRowStatus { get; set; }
	///<summary>创建人</summary>
	public string IBICreaterName { get; set; }
	///<summary>创建人工号</summary>
	public string IBICreaterWorkNumber { get; set; }
	///<summary>创建时间</summary>
	public DateTime IBICreateTime { get; set; }
	///<summary>审核人</summary>
	public string IBIAuditName { get; set; }
	///<summary>审核人工号</summary>
	public string IBIAuditWorkNumber { get; set; }
	///<summary>审核时间</summary>
	public DateTime IBIAuditTime { get; set; }
	///<summary>备注</summary>
	public string IBIRemark { get; set; }
	///<summary>图片大小,单位KB</summary>
	public int IBIImageSize { get; set; }
	///<summary>图片高度</summary>
	public int IBIImageHeight { get; set; }
	///<summary>图片宽度</summary>
	public int IBIImageWidth { get; set; }
	///<summary>图片类型，0未知，1景点、门票，2酒店，3广告、活动，4美食、购物</summary>
	public int IBIImageType { get; set; }
	///<summary>国家</summary>
	public string IBICountry { get; set; }
	///<summary>城市</summary>
	public string IBICity { get; set; }
	///<summary>有效期</summary>
	public DateTime IBIExpiresDate { get; set; }
	///<summary>上传人类型，1-内部员工，2-供应商</summary>
	public int IBIPersonType { get; set; }
	///<summary>图片驳回原因</summary>
	public string IBIRefuseReasion { get; set; }
	#endregion
}
