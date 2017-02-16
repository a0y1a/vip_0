using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using FluentValidation;
using KeKeSoftPlatform.Common;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace KeKeSoftPlatform.Core
{
    #region 用户登录
    public class SystemLoginData
    {
        [Display(Name = "账号")]
        public string Code { get; set; }

        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "验证码")]
        public string Captcha { get; set; }

    }

    #endregion

    #region 功能模块一

    #region 增删改查

    #region 学生列表
    public class StudentListData
    {
        public Guid StudentId { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public string Mobile { get; set; }
        public string ClassName { get; set; }
        public string Code { get; set; }
        public bool IsZhuXiao { get; set; }
        public StudentStatus Status { get; set; }
    }

    #endregion

    #region 添加学生

    [Validator(typeof(CreateStudentDataValidator))]
    public class CreateStudentData
    {
        [Display(Name = "班级：")]
        public Guid ClassId { get; set; }

        [Display(Name = "姓名：")]
        public string Name { get; set; }

        [Display(Name = "手机号：")]
        public string Mobile { get; set; }

        [Display(Name = "学号：")]
        public string Code { get; set; }

        [Display(Name = "性别：")]
        public Sex Sex { get; set; }

        [Display(Name = "出生日期：")]
        public string Birthday { get; set; }

        [Display(Name = "缴费：")]
        public decimal Amount { get; set; }

        [Display(Name = "是否住校：")]
        public bool IsZhuXiao { get; set; }

        [Display(Name = "地址：")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "个人描述：")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }

    }

    public class CreateStudentDataValidator : AbstractValidator<CreateStudentData>
    {
        public CreateStudentDataValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("请输入姓名");
            RuleFor(m => m.Code).NotEmpty()
                                .WithMessage("请输入学号")
                                .Matches(@"^[0-9]{6}$")
                                .WithMessage("学号必须为六位数字")
                                .Must((m, n) =>
                                {
                                    using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
                                    {
                                        if (db.Student.Any(k => k.Code == m.Code && k.ClassId == m.ClassId))
                                        {
                                            return false;
                                        }
                                        return true;
                                    }
                                }).WithMessage("输入的学号在所选班级中已经存在！");
            RuleFor(m => m.ClassId).NotEmpty().WithMessage("请选择班级");
            RuleFor(m => m.Sex).NotEmpty().WithMessage("请选择性别");
            RuleFor(m => m.Birthday).NotEmpty().WithMessage("请选择生日");
            RuleFor(m => m.Mobile).Matches(@"^1[3|4|5|8][0-9]{9}$").WithMessage("请输入正确的手机号！");
            RuleFor(m => m.Amount).NotNull().GreaterThanOrEqualTo(0);
        }
    }

    #endregion

    #region 编辑学生

    [Validator(typeof(EditStudentDataValidator))]
    public class EditStudentData
    {
        public Guid StudentId { get; set; }

        [Display(Name = "班级：")]
        public Guid ClassId { get; set; }

        [Display(Name = "学号：")]
        public string Code { get; set; }

        [Display(Name = "姓名：")]
        public string Name { get; set; }

        [Display(Name = "手机号：")]
        public string Mobile { get; set; }

        [Display(Name = "性别：")]
        public Sex Sex { get; set; }

        [Display(Name = "出生日期：")]
        public string Birthday { get; set; }

        [Display(Name = "是否住校：")]
        public bool IsZhuXiao { get; set; }

        [Display(Name = "地址：")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "个人描述：")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
    }

    public class EditStudentDataValidator : AbstractValidator<EditStudentData>
    {
        public EditStudentDataValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("请输入姓名");
            RuleFor(m => m.ClassId).NotEmpty().WithMessage("请选择班级");
            RuleFor(m => m.Sex).NotEmpty().WithMessage("请选择性别");
            RuleFor(m => m.Birthday).NotEmpty().WithMessage("请选择生日");
            RuleFor(m => m.Mobile).Matches(@"^1[3|4|5|8][0-9]{9}$").WithMessage("请输入正确的手机号！");
        }
    }

    #endregion

    #region 学生详情

    public class StudentDetailData
    {
        [Display(Name = "学号：")]
        public string Code { get; set; }

        [Display(Name = "班级：")]
        public string ClassName { get; set; }

        [Display(Name = "姓名：")]
        public string Name { get; set; }

        [Display(Name = "手机号：")]
        public string Mobile { get; set; }

        [Display(Name = "性别：")]
        public Sex Sex { get; set; }

        [Display(Name = "出生日期：")]
        public DateTime Birthday { get; set; }

        [Display(Name = "是否住校：")]
        public bool IsZhuXiao { get; set; }

        [Display(Name = "地址：")]
        public string Address { get; set; }

        [Display(Name = "个人描述：")]
        public string Description { get; set; }
    }

    #endregion

    #region 重置密码

    [Validator(typeof(ResetStudentPwdDataValidator))]
    public class ResetStudentPwdData
    {
        public Guid StudentId { get; set; }

        [Display(Name = "学号：")]
        public string Code { get; set; }

        [Display(Name = "姓名：")]
        public string Name { get; set; }

        [IsRequired]
        [DataType(DataType.Password)]
        [Display(Name = "新密码：")]
        public string NewPwd { get; set; }

        [IsRequired]
        [DataType(DataType.Password)]
        [Display(Name = "确认新密码：")]
        public string ConfirmNewPwd { get; set; }
    }

    public class ResetStudentPwdDataValidator : AbstractValidator<ResetStudentPwdData>
    {
        public ResetStudentPwdDataValidator()
        {
            RuleFor(m => m.NewPwd).NotEmpty().WithMessage("请输入新密码");
            RuleFor(m => m.ConfirmNewPwd).NotEmpty().WithMessage("请输入确认新密码").Equal(m => m.NewPwd).WithMessage("确认新密码必须和新密码一致");
        }
    }

    #endregion

    #endregion

    #region Excel操作

    public class UnionLeftJoinDownloadData
    {
        public string AuthorName { get; set; }
        public int AuthorAge { get; set; }
        public string BookName { get; set; }
        public int? BookPage { get; set; }
        public decimal? BookPrice { get; set; }
    }

    #endregion

    #region 数据验证

    [Validator(typeof(ValiateDataValidator))]
    public class ValiateData
    {
        [Display(Name = "验证密码：")]
        [IsRequired]
        public string ValidatePwd { get; set; }

        [IsRequired]
        [Display(Name = "验证重复密码一致：")]
        public string ValidateConfirmPwd { get; set; }

        [IsRequired]
        [Display(Name = "验证手机号：")]
        public string ValidateMobile { get; set; }

        [IsRequired]
        [Display(Name = "验证整数：")]
        public string ValidateInteger { get; set; }

        [IsRequired]
        [Display(Name = "验证正整数：")]
        public string ValidatePositiveInteger { get; set; }

        [IsRequired]
        [Display(Name = "验证非负整数：")]
        public string ValidateNonNegativeInteger { get; set; }

        [IsRequired]
        [Display(Name = "验证实数：")]
        public string ValidateRealNumber { get; set; }

        [IsRequired]
        [Display(Name = "验证正实数：")]
        public string ValidatePositiveRealNumber { get; set; }

        [IsRequired]
        [Display(Name = "验证非负实数：")]
        public string ValidateNonNegativeRealNumber { get; set; }

        [IsRequired]
        [Display(Name = "验证邮编：")]
        public string ValidatePostCode { get; set; }

        [IsRequired]
        [Display(Name = "验证身份证号：")]
        public string ValidateIDNumber { get; set; }

        [IsRequired]
        [Display(Name = "验证邮箱：")]
        public string ValidateEmail { get; set; }
    }

    public class ValiateDataValidator : AbstractValidator<ValiateData>
    {
        public ValiateDataValidator()
        {
            RuleFor(m => m.ValidatePwd).NotEmpty().WithMessage("请输入密码");
            //验证两次输入密码是否一致
            RuleFor(m => m.ValidateConfirmPwd).NotEmpty().WithMessage("请输入确认密码").Equal(m => m.ValidatePwd).WithMessage("确认密码和密码不一致");
            //验证手机号
            RuleFor(m => m.ValidateMobile).NotEmpty().WithMessage("请输入手机号").Matches(@"^1[3|4|5|7|8]\d{9}$").WithMessage("输入的手机号格式有误");
            //验证整数
            RuleFor(m => m.ValidateInteger).NotEmpty().WithMessage("请输入整数").Matches(@"^([1-9]\d*)|(-[1-9]\d*)|0$").WithMessage("输入的整数有误");
            //验证正整数
            RuleFor(m => m.ValidatePositiveInteger).NotEmpty().WithMessage("请输入正整数").Matches(@"^[1-9]\d*$").WithMessage("输入的正整数有误");
            //验证非负整数
            RuleFor(m => m.ValidateNonNegativeInteger).NotEmpty().WithMessage("请输入非负整数").Matches(@"^(0|[1-9]\d*)$").WithMessage("输入的非负整数有误");
            //验证实数
            RuleFor(m => m.ValidateRealNumber).NotEmpty().WithMessage("请输入实数").Matches(@"^(-)?(([1-9]\d*)|([1-9]\d*\.[0-9]\d*)|(0\.[0-9]+)|0)$").WithMessage("输入的实数有误");
            //验证正实数
            RuleFor(m => m.ValidatePositiveRealNumber).NotEmpty().WithMessage("请输入正实数").Matches(@"^(([1-9]\d*)|([1-9]\d*\.[0-9]\d*)|(0\.\d*[1-9]\d*))$").WithMessage("输入的正实数有误");
            //验证非负实数
            RuleFor(m => m.ValidateNonNegativeRealNumber).NotEmpty().WithMessage("请输入非负实数").Matches(@"^(([1-9]\d*)|([1-9]\d*\.[0-9]\d*)|(0\.[0-9]+)|0)$").WithMessage("输入的非负实数有误");
            //验证邮编
            RuleFor(m => m.ValidatePostCode).NotEmpty().WithMessage("请输入邮编").Matches(@"^\d{6}$").WithMessage("输入的邮编格式有误");
            //验证身份证号
            RuleFor(m => m.ValidateIDNumber).NotEmpty().WithMessage("请输入身份证号")
                                            .Matches(@"^[0-9]{17}([0-9]|X)$").WithMessage("输入的身份证号格式有误")
                                            .Must(k =>
                                            {
                                                int[] iW = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
                                                int iSum = 0;
                                                var v_card = k;
                                                for (var i = 0; i < 17; i++)
                                                {
                                                    var iC = v_card.Substring(i, 1);
                                                    var iVal = Convert.ToInt32(iC);
                                                    iSum += iVal * iW[i];
                                                }
                                                var iJYM = iSum % 11;
                                                var sJYM = "";
                                                if (iJYM == 0) sJYM = "1";
                                                else if (iJYM == 1) sJYM = "0";
                                                else if (iJYM == 2) sJYM = "x";
                                                else if (iJYM == 3) sJYM = "9";
                                                else if (iJYM == 4) sJYM = "8";
                                                else if (iJYM == 5) sJYM = "7";
                                                else if (iJYM == 6) sJYM = "6";
                                                else if (iJYM == 7) sJYM = "5";
                                                else if (iJYM == 8) sJYM = "4";
                                                else if (iJYM == 9) sJYM = "3";
                                                else if (iJYM == 10) sJYM = "2";
                                                var cCheck = v_card.Substring(17, 1).ToLower();
                                                if (cCheck != sJYM)
                                                {
                                                    return false;
                                                }
                                                return true;
                                            }).WithMessage("您输入的身份证号格式不对!");

            ////验证实数，必须匹配小数点
            //RuleFor(m => m.ValidateRealNumber).NotEmpty().WithMessage("请输入实数").Matches(@"^-?(([1-9]\d*)|0)\.\d*$").WithMessage("输入的实数有误");
            ////验证正实数，必须匹配小数点
            //RuleFor(m => m.ValidatePositiveRealNumber).NotEmpty().WithMessage("请输入正实数").Matches(@"^(([1-9]\d*)|0)\.\d*$").WithMessage("输入的正实数有误");


            //验证邮箱
            //RuleFor(m => m.ValidateEmail).NotEmpty().WithMessage("请输入邮箱").Matches(@"^\s*([A-Za-z0-9_-]+(\.\w+)*@(\w+\.)+\w{2,5})\s*$").WithMessage("输入的邮箱格式有误");
        }
    }

    #endregion

    #region 文本编辑
    public class CreateNewsData
    {
        public NewsType Type { get; set; }

        [AllowHtml]
        [Required]
        public string Content { get; set; }
    }

    #endregion

    #region 下拉查询

    [Validator(typeof(Select2DataValidator))]
    public class Select2Data
    {
        public Guid StudentId { get; set; }

        [Display(Name = "更换班级：")]
        public Guid ClassId { get; set; }

        [Display(Name = "学生角色：")]
        public string RoleIdList { get; set; }
    }

    public class Select2DataValidator : AbstractValidator<Select2Data>
    {
        public Select2DataValidator()
        {
            RuleFor(m => m.ClassId).NotEmpty().WithMessage("请选择班级");
            RuleFor(m => m.RoleIdList).NotEmpty().WithMessage("请选择角色");
        }
    }

    #endregion

    #region 省市区域

    public class PCCItemData
    {
        public Int64 Id { get; set; }
        public Int64? ParentId { get; set; }
        public string Name { get; set; }
    }

    public class PCCDisplayData
    {
        public Int64 ProvinceId { get; set; }
        public Int64 CityId { get; set; }
        public Int64 CountyId { get; set; }
    }

    #endregion

    #region 文件上传

    public class UploadifyFirstData
    {
        public string AddImageList { get; set; }

        [Display(Name = "上传文件")]
        public string AddImageNameList { get; set; }
    }

    public class UploadifySecondData
    {
        public string AddImageList { get; set; }

        public List<string> FileDataList { get; set; }

        public UploadifySecondData()
        {
            FileDataList = new List<string>();
        }
    }

    #endregion

    #region 内存分页
    public class TestPager
    {
        /// <summary>
        /// 数据总条数
        /// </summary>
        public int ItemCount { get; set; }
        /// <summary>
        /// 首页多少条
        /// </summary>
        public int FirstPageSize { get; set; }
        /// <summary>
        /// 每页多少条
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 当前页页码
        /// </summary>
        public int PageNum { get; set; }
        /// <summary>
        /// 一共多少页
        /// </summary>
        public int PageCount
        {
            get
            {
                return (int)Math.Ceiling((double)ItemCount / (double)PageSize);
            }
        }

        public List<T_Class> ClassList { get; set; }

        public TestPager()
        {
            ClassList = new List<T_Class>();
        }
    }

    #endregion

    #region 编辑班级

    [Validator(typeof(EditClassDataValidator))]
    public class EditClassData
    {
        public Guid ClassId { get; set; }
        [Display(Name = "编码")]
        public string Code { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "类型")]
        public ClassType Type { get; set; }

        [AllowHtml]
        [Display(Name = "描述")]
        public string Description { get; set; }
    }

    public class EditClassDataValidator : AbstractValidator<EditClassData>
    {
        public EditClassDataValidator()
        {
            RuleFor(m => m.Code).NotEmpty()
                                .WithMessage("班级编码不能为空！")
                                .Matches(@"^\d{3}$")
                                .WithMessage("班级编码必须为三位数字！")
                                .Must((m, n) =>
                                {
                                    using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
                                    {
                                        if (db.Class.Any(k => k.Code == m.Code && k.Id != m.ClassId))
                                        {
                                            return false;
                                        }
                                        return true;
                                    }
                                }).WithMessage("班级编码已经存在！");

            RuleFor(m => m.Name).NotEmpty()
                                .WithMessage("班级名称不能空！")
                                .Must((m, n) =>
                                {
                                    using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
                                    {
                                        if (db.Class.Any(k => k.Name == m.Name.Trim() && k.Id != m.ClassId))
                                        {
                                            return false;
                                        }
                                        return true;
                                    }
                                }).WithMessage("班级名称已经存在！");
        }
    }

    #endregion

    #endregion

    #region 功能模块二

    public class SaveImgageData
    {
        public string AddImageList { get; set; }

        public List<ImageData> ImageDataList { get; set; }

        public SaveImgageData()
        {
            ImageDataList = new List<ImageData>();
        }
    }

    public class ImageData
    {
        public string Url { get; set; }
        public byte[] ImgageByte { get; set; }
    }

    public class OperatePageData
    {
        public string DropdownListTest { get; set; }
        public string SelectOptionTest { get; set; }
        public string InputGroupTest { get; set; }
    }

    public class SelectColorData
    {
        [Display(Name = "选择颜色一：")]
        public string Color1 { get; set; }

        [Display(Name = "选择颜色二：")]
        public string Color2 { get; set; }
    }

    #endregion

    #region 测试数据

    [Validator(typeof(TestDataValidator))]
    public class TestData
    {
        [Display(Name = "姓名：")]
        public string Name { get; set; }

        [Display(Name = "编号：")]
        public string Code { get; set; }

        [Display(Name = "成绩1：")]
        public decimal Score1 { get; set; }

        [Display(Name = "成绩2：")]
        public decimal Score2 { get; set; }

        [Display(Name = "年龄1：")]
        public int Age1 { get; set; }

        [Display(Name = "年龄2：")]
        public int Age2 { get; set; }

        [Display(Name = "Sex")]
        public bool Sex { get; set; }
        public string SerializeObject { get; set; }
    }

    public class TestDataValidator : AbstractValidator<TestData>
    {
        public TestDataValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("姓名不能为空!");
            RuleFor(m => m.Code).NotNull().WithMessage("编号不能为空!");
            RuleFor(m => m.Score1).NotEmpty().WithMessage("成绩1不能为空!");
            RuleFor(m => m.Score2).NotNull().WithMessage("成绩2不能为空!");
            RuleFor(m => m.Age1).NotEmpty().WithMessage("年龄1不能为空!");
            RuleFor(m => m.Age2).NotNull().WithMessage("年龄2不能为空!");
        }
    }

    public class StudentData
    {
        public string Name { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public decimal Height { get; set; }
    }

    public class StoreData
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class SerializeStudentData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class UploadFileData
    {
        public string Type { get; set; }

        public string Caption { get; set; }

        public long Size { get; set; }

        public string Key { get; set; }

        public string Width { get; set; }

        public string Height { get; set; }
    }

    #endregion
}
