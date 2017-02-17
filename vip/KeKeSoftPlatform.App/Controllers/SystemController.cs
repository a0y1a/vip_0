using KeKeSoftPlatform.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeKeSoftPlatform.Core;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Data.Entity;
using System.Web.Security;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
using System.Threading.Tasks;
using System.Drawing;
using System.Text;
using EntityFramework.Extensions;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace KeKeSoftPlatform.App.Controllers
{
    [Right(RightType.Admin)]
    public class SystemController : BaseController
    {
        #region 登录

        [Right(RightType.Anonymous)]
        public ActionResult Login()
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var admin = db.Admin.FirstOrDefault();

                FormsPrincipal<_User>.SignIn(admin.Code, new _User
                {
                    Id = admin.Id,
                    Name = admin.Code,
                    RightType = RightType.Admin
                });

                return RedirectToAction("StudentList");
            }
        }

        #endregion

        #region 退出
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        #endregion
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
        #region 首页
        public ActionResult Home()
        {
            return View();
        }

        #endregion

        #region 功能模块一

        #region 01. 增删改查

        #region 学生列表
        public ActionResult StudentList(string name, Sex? sex, Guid? classId, DateTime? maxDate, DateTime? minDate, int pageNum = 1)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                IQueryable<T_Student> query = db.Student;

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(m => m.Name.Trim().Contains(name.Trim()));
                }
                if (sex.HasValue)
                {
                    query = query.Where(m => m.Sex == sex.Value);
                }
                if (minDate.HasValue)
                {
                    query = query.Where(m => m.Birthday >= minDate.Value);
                }
                if (maxDate.HasValue)
                {
                    query = query.Where(m => m.Birthday <= maxDate.Value);
                }
                if (classId.HasValue)
                {
                    query = query.Where(m => m.ClassId == classId.Value);
                }

                return View(query.Select(m => new StudentListData
                {
                    StudentId = m.Id,
                    Name = m.Name,
                    Sex = m.Sex,
                    Code = m.Code,
                    ClassName = m.Class.Name,
                    Birthday = m.Birthday,
                    Age = m.Age,
                    Mobile = m.Mobile,
                    IsZhuXiao = m.IsZhuXiao,
                    Status = m.Status
                }).OrderBy(m => m.Code).Page<StudentListData>(pageNum));
            }
        }

        #endregion

        #region 添加学生

        public ActionResult CreateStudent()
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                if (db.Class.Any() == false)
                {
                    TempData[AlertEntity.ALERT_ENTITY] = new AlertEntity("请先添加班级，再添加学生！", AlertType.Danger);
                    return Redirect(Request.UrlReferrer.AbsoluteUri);
                }
            }
            return View(new CreateStudentData { Birthday = DateTime.Now.ToString("yyyy-MM-dd") });
        }

        [HttpPost]
        public ActionResult CreateStudent(CreateStudentData model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                db.Student.Add(new T_Student
                {
                    Age = DateTime.Now.Year - Convert.ToDateTime(model.Birthday).Year,
                    Birthday = Convert.ToDateTime(model.Birthday),
                    ClassId = model.ClassId,
                    Code = model.Code,
                    Amount = model.Amount,
                    Name = model.Name.Trim(),
                    Sex = model.Sex,
                    Mobile = model.Mobile,
                    Address = model.Address,
                    Description = model.Description,
                    IsZhuXiao = model.IsZhuXiao
                });
                db.SaveChanges();
                TempData[AlertEntity.ALERT_ENTITY] = new AlertEntity(Service.ActionSuccess);
                return RedirectToAction("StudentList");
            }
        }

        #endregion

        #region 编辑学生

        public ActionResult EditStudent(Guid studentId)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var student = db.Student.Find(studentId);
                return View(new EditStudentData
                {
                    StudentId = student.Id,
                    Sex = student.Sex,
                    Name = student.Name,
                    Mobile = student.Mobile,
                    ClassId = student.ClassId,
                    Code = student.Code,
                    Address = student.Address,
                    Description = student.Description,
                    Birthday = student.Birthday.ToString("yyyy-MM-dd"),
                    IsZhuXiao = student.IsZhuXiao
                });
            }
        }

        [HttpPost]
        public ActionResult EditStudent(EditStudentData model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var student = db.Student.Find(model.StudentId);

                student.Age = DateTime.Now.Year - Convert.ToDateTime(model.Birthday).Year;
                student.Birthday = Convert.ToDateTime(model.Birthday);
                student.ClassId = model.ClassId;
                student.Name = model.Name;
                student.Sex = model.Sex;
                student.Mobile = model.Mobile;
                student.IsZhuXiao = model.IsZhuXiao;
                student.Address = model.Address;
                student.Description = model.Description;

                db.SaveChanges();
                TempData[AlertEntity.ALERT_ENTITY] = new AlertEntity(Service.ActionSuccess);
                return RedirectToAction("StudentList");
            }
        }

        #endregion

        #region 学生详情

        public ActionResult StudentDetail(Guid studentId)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var student = db.Student.Find(studentId);
                return View(new StudentDetailData
                {
                    Sex = student.Sex,
                    Name = student.Name,
                    ClassName = student.Class.Name,
                    Mobile = student.Mobile,
                    Code = student.Code,
                    Address = student.Address,
                    Description = student.Description,
                    Birthday = student.Birthday,
                    IsZhuXiao = student.IsZhuXiao
                });
            }
        }

        #endregion

        #region 删除学生
        public ActionResult DeleteStudent(Guid studentId)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                db.Student.Remove(db.Student.Find(studentId));
                db.SaveChanges();
                TempData[AlertEntity.ALERT_ENTITY] = new AlertEntity(Service.ActionSuccess);
                return RedirectToAction("StudentList");
            }
        }

        #endregion

        #region 重置密码

        public ActionResult ResetStudentPwd(Guid studentId)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var student = db.Student.Find(studentId);
                return View(new ResetStudentPwdData
                {
                    StudentId = student.Id,
                    Name = student.Name,
                    Code = student.Code
                });
            }
        }

        [HttpPost]
        public ActionResult ResetStudentPwd(ResetStudentPwdData model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var student = db.Student.Find(model.StudentId);
                student.Password = EncryptUtils.MD5Encrypt(model.NewPwd);
                db.SaveChanges();
                TempData[AlertEntity.ALERT_ENTITY] = new AlertEntity(Service.ActionSuccess);
                return RedirectToAction("StudentList");
            }
        }

        #endregion

        #endregion

        #region 02. 附件上传

        #region 附件列表

        public ActionResult FileList(int pageNum = 1)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                return View(db.File.OrderBy(m => m.CreateDate).Page<T_File>(pageNum));
            }
        }

        #endregion

        #region 附件上传

        #region Uploadify_First

        public ActionResult UploadifyFirst()
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {

                return View();
            }
        }

        [HttpPost]
        public ActionResult UploadifyFirst(UploadifyFirstData model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            if (!model.AddImageList.IsNullOrWhiteSpace())
            {
                using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
                {
                    List<string> fileList = model.AddImageList.Replace("\"", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (var item in fileList.Where(m => m.Length >= 10).Distinct())
                    {
                        var fileName = System.IO.Path.GetFileName(item);
                        db.File.Add(new T_File
                        {
                            Name = fileName,
                            MiaoShu = "",
                            Url = item,
                            CreateDate = DateTime.Now,
                            FileType = FileType.Uploadify,
                            SortType = SortType.First
                        });
                    }

                    db.SaveChanges();
                    TempData[AlertEntity.ALERT_ENTITY] = new AlertEntity(Service.ActionSuccess);
                }
            }

            return RedirectToAction("FileList");
        }

        #endregion

        #region Uploadify_Second

        public ActionResult UploadifySecond()
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var model = new UploadifySecondData { };
                model.FileDataList.AddRange(db.File.Where(m => m.FileType == FileType.Uploadify && m.SortType == SortType.Second).OrderBy(m => m.CreateDate).Select(m => m.Url));
                model.AddImageList = string.Join(",,,", model.FileDataList) + ",,,";
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult UploadifySecond(UploadifySecondData model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            if (!model.AddImageList.IsNullOrWhiteSpace())
            {
                using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
                {
                    //原有附件
                    var oldFileList = db.File.Where(m => m.FileType == FileType.Uploadify && m.SortType == SortType.Second).ToList();
                    List<T_File> checkFileList = new List<T_File>();

                    //重新保存附件
                    foreach (var url in model.AddImageList.Split(new string[] { ",,," }, StringSplitOptions.RemoveEmptyEntries).Where(m => m.Length >= 20))
                    {
                        //判断是否删除原有附件
                        if (oldFileList.Any(m => m.Url == url))
                        {
                            checkFileList.Add(oldFileList.FirstOrDefault(m => m.Url == url));
                            continue;
                        }

                        db.File.Add(new T_File
                        {
                            Name = System.IO.Path.GetFileName(url).GetOriginalName(),
                            MiaoShu = "",
                            Url = url,
                            CreateDate = DateTime.Now,
                            FileType = FileType.Uploadify,
                            SortType = SortType.Second
                        });
                    }

                    //要删除的附件
                    var deleteFileList = oldFileList.Except(checkFileList).ToList();
                    //删除原有附件
                    foreach (var item in deleteFileList)
                    {
                        if (System.IO.File.Exists(Server.MapPath(item.Url)))
                        {
                            System.IO.File.Delete(Server.MapPath(item.Url));
                        }
                    }
                    db.File.RemoveRange(deleteFileList);

                    db.SaveChanges();
                    TempData[AlertEntity.ALERT_ENTITY] = new AlertEntity(Service.ActionSuccess);
                }
            }

            return RedirectToAction("FileList");
        }

        #endregion

        #endregion

        #region 附件管理

        [Right(RightType.Anonymous)]
        public JsonResult Upload()
        {
            var path = "";
            if (!Request.Files[0].FileName.IsNullOrWhiteSpace())
            {
                var fileName = Path.GetFileNameWithoutExtension(Request.Files[0].FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + _User.Current.Id.ToString().Substring(3, 5) + Path.GetExtension(Request.Files[0].FileName);
                var filePath = Server.MapPath(@"~/Upload/File/{0}".FormatString(fileName));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                Request.Files[0].SaveAs(filePath);

                //将文件的物理路径转化成虚拟路径
                path = "/" + filePath.Substring(AppDomain.CurrentDomain.BaseDirectory.Length).Replace(@"\", @"/");
            }

            return Json(path, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteFile(Guid fuJianId)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var file = db.File.Find(fuJianId);
                if (System.IO.File.Exists(Server.MapPath(file.Url)))
                {
                    System.IO.File.Delete(Server.MapPath(file.Url));
                }
                db.File.Remove(file);
                db.SaveChanges();
                return Json(new ReturnValue { IsSuccess = true });
            }
        }

        #endregion

        #endregion

        #region 03. 数据验证
        public ActionResult DataValidation()
        {
            return View(new ValiateData
            {
                ValidatePwd = "1",
                ValidateConfirmPwd = "1",
                ValidateMobile = "15275578152",
                ValidateInteger = "11",
                ValidatePositiveInteger = "11",
                ValidateNonNegativeInteger = "0",
                ValidateRealNumber = "1.1",
                ValidatePositiveRealNumber = "1.112",
                ValidateNonNegativeRealNumber = "0.1",
                ValidatePostCode = "123456",
                ValidateIDNumber = "371311198801102013"
            });
        }

        #endregion

        #region 04. Excel操作

        #region Excel操作

        public ActionResult OperateExcel()
        {
            return View();
        }

        #endregion

        #region 下载模板
        public FileResult DownLoadStudentTemplate()
        {
            return Download(Server.MapPath(@"~/Upload/Template/学生模板.xls"));
        }

        #endregion

        #region 导入数据
        public ActionResult ImportStudentData()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportStudentData(string import)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var file = Request.Files[0];

                if (file == null)
                {
                    TempData[AlertEntity.ALERT_ENTITY] = new AlertEntity("请上传文件！", AlertType.Danger);
                    return View();
                }

                var newName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                var path = Server.MapPath(@"~/Upload/Import/" + newName);
                file.SaveAs(path);

                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
                {
                    IWorkbook workBook = null;

                    if (fs.Name.LastIndexOf(".xlsx") > 0)
                    {
                        workBook = new XSSFWorkbook(fs);
                    }
                    else if (fs.Name.LastIndexOf(".xls") > 0)
                    {
                        workBook = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        TempData[AlertEntity.ALERT_ENTITY] = new AlertEntity("格式有误，读取文件失败！", AlertType.Danger);
                        return View();
                    }

                    ISheet sheet = workBook.GetSheetAt(0);
                    string errorMessage = string.Empty;
                    //查询所有的学生
                    var studentCollection = db.Student.ToDictionary(m => m.Code);
                    //查询所有的班级
                    var classCollection = db.Class.ToDictionary(m => m.Name);
                    //保存导入的学生
                    List<T_Student> studentList = new List<T_Student>();
                    //性别列表
                    var sexTypeList = Sex.Female.ToSelectDataSource();
                    //错误和成功数目
                    int errorCnt = 0, successCnt = 0;
                    //是否成功,全部数据正确是才会导入
                    bool isSuccess = true;

                    #region 核查数据
                    for (int j = 1; j <= sheet.LastRowNum; j++)
                    {
                        IRow row = sheet.GetRow(j);
                        T_Student student = new T_Student();

                        //班级
                        ICell classCell = row.GetCell(5);
                        if (classCell == null || string.IsNullOrWhiteSpace(classCell.StringCellValue.Trim()))
                        {
                            errorMessage += "第{0}行数据中的班级为空！".FormatString(j + 1);
                            isSuccess = false;
                            errorCnt++;
                            continue;
                        }
                        if (classCollection.ContainsKey(classCell.StringCellValue.Trim()) == false)
                        {
                            errorMessage += "第{0}行数据中的班级名称有误！".FormatString(j + 1);
                            isSuccess = false;
                            errorCnt++;
                            continue;
                        }

                        T_Class @class = classCollection[classCell.StringCellValue.Trim()];

                        //学号
                        ICell codeCell = row.GetCell(4);
                        if (codeCell == null || string.IsNullOrWhiteSpace(codeCell.ToString().Trim()))
                        {
                            errorMessage += "第{0}行数据中的学号为空！".FormatString(j + 1);
                            isSuccess = false;
                            errorCnt++;
                            continue;
                        }
                        if (Regex.IsMatch(codeCell.ToString().Trim(), @"^\d{6}$") == false)
                        {
                            errorMessage += "第{0}行数据中的学号有误！".FormatString(j + 1);
                            isSuccess = false;
                            errorCnt++;
                            continue;
                        }
                        if (codeCell.StringCellValue.Trim().Substring(0, 3) != @class.Code)
                        {
                            errorMessage += "第{0}行数据中的学号与班级编码不匹配！".FormatString(j + 1);
                            isSuccess = false;
                            errorCnt++;
                            continue;
                        }

                        //姓名
                        ICell nameCell = row.GetCell(0);
                        if (nameCell == null || string.IsNullOrWhiteSpace(nameCell.ToString().Trim()))
                        {
                            errorMessage += "第{0}行数据中的姓名为空！".FormatString(j + 1);
                            isSuccess = false;
                            errorCnt++;
                            continue;
                        }

                        //性别
                        ICell sexTypeCell = row.GetCell(1);
                        if (sexTypeCell == null || string.IsNullOrWhiteSpace(sexTypeCell.ToString().Trim()))
                        {
                            errorMessage += "第{0}行数据中的性别为空！".FormatString(j + 1);
                            isSuccess = false;
                            errorCnt++;
                            continue;
                        }
                        if (sexTypeList.Any(m => m.Text == sexTypeCell.ToString().Trim()) == false)
                        {
                            errorMessage += "第{0}行数据中的性别有误！".FormatString(j + 1);
                            isSuccess = false;
                            errorCnt++;
                            continue;
                        }

                        //年龄
                        ICell ageCell = row.GetCell(2);
                        int age = 0;
                        if (ageCell == null || string.IsNullOrWhiteSpace(ageCell.ToString().Trim()))
                        {
                            errorMessage += "第{0}行数据中的年龄为空！".FormatString(j + 1);
                            isSuccess = false;
                            errorCnt++;
                            continue;
                        }
                        if (Int32.TryParse(ageCell.ToString().Trim(), out age) == false || age <= 0)
                        {
                            errorMessage += "第{0}行数据中的年龄有误！".FormatString(j + 1);
                            isSuccess = false;
                            errorCnt++;
                            continue;
                        }

                        //生日
                        ICell birthdayCell = row.GetCell(3);
                        if (birthdayCell == null || string.IsNullOrWhiteSpace(birthdayCell.ToString().Trim()))
                        {
                            errorMessage += "第{0}行数据中的出生日期为空！".FormatString(j + 1);
                            isSuccess = false;
                            errorCnt++;
                            continue;
                        }
                        string birthdayCellString = birthdayCell.ToString().Trim();
                        DateTime birthday = DateTime.MinValue;
                        if (birthdayCellString.Length != 8 || DateTime.TryParse(birthdayCellString.Substring(0, 4) + "-" + birthdayCellString.Substring(4, 2) + "-" + birthdayCellString.Substring(6, 2), out birthday) == false)
                        {
                            errorMessage += "第{0}行数据中的出生日期有误！".FormatString(j + 1);
                            isSuccess = false;
                            errorCnt++;
                            continue;
                        }
                    }
                    #endregion

                    #region 保存数据
                    if (isSuccess)
                    {
                        for (int j = 1; j <= sheet.LastRowNum; j++)
                        {
                            IRow row = sheet.GetRow(j);
                            T_Student student = new T_Student();
                            bool isNew = false;
                            T_Class @class = classCollection[row.GetCell(5).StringCellValue.Trim()];

                            #region 保存数据

                            //保存学号
                            ICell codeCell = row.GetCell(4);
                            if (@class.Student.Any(m => m.Code == codeCell.StringCellValue.Trim()))
                            {
                                student = studentCollection[codeCell.ToString().Trim()];
                                isNew = true;
                            }
                            else
                            {
                                student.Code = codeCell.ToString().Trim();
                            }

                            //保存姓名
                            student.Name = row.GetCell(0).ToString().Trim();

                            //保存性别
                            student.Sex = row.GetCell(1).ToString().Trim().EnumDisplayToEnum<Sex>();

                            //保存年龄
                            student.Age = Convert.ToInt32(row.GetCell(2).ToString());

                            //保存出生日期
                            string birthdayString = row.GetCell(3).ToString().Trim();
                            student.Birthday = Convert.ToDateTime(birthdayString.Substring(0, 4) + "-" + birthdayString.Substring(4, 2) + "-" + birthdayString.Substring(6, 2));

                            if (isNew == false)
                            {
                                student.ClassId = @class.Id;
                                studentList.Add(student);
                                studentCollection.Add(student.Code, student);
                            }

                            successCnt++;
                            #endregion
                        }

                        if (studentList.Any())
                        {
                            db.Student.AddRange(studentList);
                        }
                        db.SaveChanges();
                    }

                    errorMessage += "错误{0}处，成功{1}条".FormatString(errorCnt, successCnt);
                    TempData[AlertEntity.ALERT_ENTITY] = new AlertEntity(errorMessage);
                    return RedirectToAction("StudentList");
                    #endregion
                }
            }
        }

        #endregion

        #region 导出数据
        public void DownLoadStudentData()
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                IWorkbook workBook = new HSSFWorkbook();
                ISheet sheet = workBook.CreateSheet("学生信息表");
                IRow headingRow = sheet.CreateRow(0);
                headingRow.CreateCell(0).SetCellValue("姓名");
                headingRow.CreateCell(1).SetCellValue("性别");
                headingRow.CreateCell(2).SetCellValue("年龄");
                headingRow.CreateCell(3).SetCellValue("出生日期");
                headingRow.CreateCell(4).SetCellValue("学号");

                var studentList = db.Student.ToList();
                for (int i = 0; i < studentList.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    row.CreateCell(0).SetCellValue(studentList[i].Name);
                    row.CreateCell(1).SetCellValue(studentList[i].Sex.EnumMetadataDisplay());
                    row.CreateCell(2).SetCellValue(studentList[i].Age);
                    row.CreateCell(3).SetCellValue(studentList[i].Birthday.ToString("yyyy-MM-dd"));
                    row.CreateCell(4).SetCellValue(studentList[i].Code);
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "UTF-8";
                Response.ClearHeaders();
                string outputFileName = "学生信息表.xls";

                //火狐浏览器下不对标题进行编码，其它浏览器下对标题进行编码，避免出现标题乱码现象
                if (Request.UserAgent.ToUpper().Contains("FIREFOX") == false)
                {
                    outputFileName = Url.Encode(outputFileName);
                }

                Response.AppendHeader("Content-Disposition", "attachment;filename=" + outputFileName);   //attachment，下载；inline 在线打开
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                Response.ContentType = "application/ms-excel";

                using (MemoryStream ms = new MemoryStream())
                {
                    //将工作簿的内容放到内存流中
                    workBook.Write(ms);
                    //将内存流转换成字节数组发送到客户端
                    Response.BinaryWrite(ms.GetBuffer());
                    Response.End();
                }
            }
        }

        #endregion

        #region 联合表内连接导出数据

        public void UnionInnerJoinDownload()
        {
            //一个作者可能对应一本或多本书籍，查询结果中不显示没有对应书籍的作者和没有对应作者的书籍
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                //方法一，使用方法语句
                var query = db.Author.Join(db.Book, m => m.Id, n => n.AuthorId, (m, n) => new { m, n })
                                    .Select(k => new
                                    {
                                        AuthorName = k.m.Name,
                                        AuthorAge = k.m.Age,
                                        BookName = k.n.Name,
                                        BookPage = k.n.Page,
                                        BookPrice = k.n.Price
                                    }).OrderBy(m => m.AuthorName).ToList();

                //方法二，使用查询语句
                //var query2 = (from a in db.Author
                //             join b in db.Book
                //             on a.Id equals b.AuthorId
                //             select new
                //             {
                //                 AuthorName = a.Name,
                //                 AuthorAge = a.Age,
                //                 BookName = b.Name,
                //                 BookPage = b.Page,
                //                 BookPrice = b.Price
                //             }).OrderBy(m => m.AuthorName).ToList();

                IWorkbook workBook = new HSSFWorkbook();
                ISheet sheet = workBook.CreateSheet("UnionInnerJoinDownload");
                IRow row = sheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("AuthorName");
                row.CreateCell(1).SetCellValue("AuthorAge");
                row.CreateCell(2).SetCellValue("BookName");
                row.CreateCell(3).SetCellValue("BookPage");
                row.CreateCell(4).SetCellValue("BookPrice");

                for (int i = 0; i < query.Count; i++)
                {
                    IRow tmpRow = sheet.CreateRow(i + 1);
                    tmpRow.CreateCell(0).SetCellValue(query[i].AuthorName);
                    tmpRow.CreateCell(1).SetCellValue(query[i].AuthorAge);
                    tmpRow.CreateCell(2).SetCellValue(query[i].BookName);
                    tmpRow.CreateCell(3).SetCellValue(query[i].BookPage);
                    tmpRow.CreateCell(4).SetCellValue(query[i].BookPrice.ToString("N2"));
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "utf-8";
                Response.ClearHeaders();
                string outputFileName = "UnionInnerJoinDownload.xls";

                if (Request.UserAgent.ToUpper().Contains("FIREFOX") == false)
                {
                    outputFileName = Url.Encode(outputFileName);
                }

                Response.AddHeader("Content-Disposition", "attachment;filename=" + outputFileName);
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
                Response.ContentType = "application/ms-excel";

                using (MemoryStream ms = new MemoryStream())
                {
                    workBook.Write(ms);
                    Response.BinaryWrite(ms.GetBuffer());
                    Response.End();
                }
            }
        }

        #endregion

        #region 联合表左连接导出数据

        public void UnionLeftJoinDownload()
        {
            //一个作者可能对应一本或多本书籍，若没有对应的书籍，则书籍的相关信息设置为空
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                //方法一，使用Linq
                var query = (from a in db.Author
                             join b in db.Book
                             on a.Id equals b.AuthorId into ab
                             from t in ab.DefaultIfEmpty()
                             select new
                             {
                                 AuthorName = a.Name,
                                 AuthorAge = a.Age,
                                 BookName = t == null ? "" : t.Name,
                                 BookPage = t == null ? "" : t.Page.ToString(),
                                 BookPrice = t == null ? "" : t.Price.ToString()
                             }).OrderBy(m => m.AuthorName).ToList();

                //方法二，使用sql查询语句
                var query2 = db.Database.SqlQuery<UnionLeftJoinDownloadData>(@"SELECT ta.Name AS AuthorName, 
                                                                                      ta.Age AS AuthorAge, 
                                                                                      tb.Name AS BookName, 
                                                                                      tb.[Page] AS BookPage,  
                                                                                      tb.Price AS BookPrice
                                                                                FROM T_Author AS ta
                                                                                LEFT JOIN T_Book AS tb
                                                                                ON ta.Id = tb.AuthorId
                                                                                WHERE ta.Age < @age
                                                                                ORDER BY ta.Name", new SqlParameter("@age", 100)).ToList();

                IWorkbook workBook = new HSSFWorkbook();
                ISheet sheet = workBook.CreateSheet("UnionLeftJoinDownload");
                IRow row = sheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("AuthorName");
                row.CreateCell(1).SetCellValue("AuthorAge");
                row.CreateCell(2).SetCellValue("BookName");
                row.CreateCell(3).SetCellValue("BookPage");
                row.CreateCell(4).SetCellValue("BookPrice");

                for (int i = 0; i < query.Count; i++)
                //for (int i = 0; i < query2.Count; i++)
                {
                    IRow tmpRow = sheet.CreateRow(i + 1);
                    tmpRow.CreateCell(0).SetCellValue(query[i].AuthorName);
                    tmpRow.CreateCell(1).SetCellValue(query[i].AuthorAge);
                    tmpRow.CreateCell(2).SetCellValue(query[i].BookName);
                    tmpRow.CreateCell(3).SetCellValue(query[i].BookPage);
                    tmpRow.CreateCell(4).SetCellValue(query[i].BookPrice);
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "utf-8";
                Response.ClearHeaders();
                string outputFileName = "UnionLeftJoinDownload.xls";

                if (Request.UserAgent.ToUpper().Contains("FIREFOX") == false)
                {
                    outputFileName = Url.Encode(outputFileName);
                }

                Response.AddHeader("Content-Disposition", "attachment;filename=" + outputFileName);
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
                Response.ContentType = "application/ms-excel";

                using (MemoryStream ms = new MemoryStream())
                {
                    workBook.Write(ms);
                    Response.BinaryWrite(ms.GetBuffer());
                    Response.End();
                }
            }
        }

        #endregion

        #region 联合表全连接导出数据

        public void UnionFullOuterJoinDownload()
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var query = (from a in db.Author
                             join b in db.Book
                             on a.Id equals b.AuthorId into ab
                             from t in ab.DefaultIfEmpty()
                             select new
                             {
                                 AuthorName = a.Name,
                                 AuthorAge = a.Age,
                                 BookName = t == null ? "" : t.Name,
                                 BookPage = t == null ? "" : t.Page.ToString(),
                                 BookPrice = t == null ? "" : t.Price.ToString()
                             }).OrderBy(m => m.AuthorName).ToList();

                IWorkbook workBook = new HSSFWorkbook();
                ISheet sheet = workBook.CreateSheet("author-book-information-list");
                IRow row = sheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("AuthorName");
                row.CreateCell(1).SetCellValue("AuthorAge");
                row.CreateCell(2).SetCellValue("BookName");
                row.CreateCell(3).SetCellValue("BookPage");
                row.CreateCell(4).SetCellValue("BookPrice");

                for (int i = 0; i < query.Count; i++)
                {
                    IRow tmpRow = sheet.CreateRow(i + 1);
                    tmpRow.CreateCell(0).SetCellValue(query[i].AuthorName);
                    tmpRow.CreateCell(1).SetCellValue(query[i].AuthorAge);
                    tmpRow.CreateCell(2).SetCellValue(query[i].BookName);
                    tmpRow.CreateCell(3).SetCellValue(query[i].BookPage);
                    tmpRow.CreateCell(4).SetCellValue(query[i].BookPrice);
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "utf-8";
                Response.ClearHeaders();
                string outputFileName = "author-book-informatin-list.xls";

                if (Request.UserAgent.ToUpper().Contains("FIREFOX") == false)
                {
                    outputFileName = Url.Encode(outputFileName);
                }

                Response.AddHeader("Content-Disposition", "attachment;filename=" + outputFileName);
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
                Response.ContentType = "application/ms-excel";

                using (MemoryStream ms = new MemoryStream())
                {
                    workBook.Write(ms);
                    Response.BinaryWrite(ms.GetBuffer());
                    Response.End();
                }
            }
        }

        #endregion

        #endregion

        #region 05. 配置文件

        public ActionResult Config()
        {
            return View(KeKeSoftPlatform.Core.Config.Instance);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Config(KeKeSoftPlatform.Core.Config model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Serialize();
            TempData[AlertEntity.ALERT_ENTITY] = new AlertEntity(Service.ActionSuccess);
            return RedirectToAction("Config");
        }

        #endregion

        #region 06. 文本编辑

        public ActionResult CreateNews()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateNews(CreateNewsData model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                db.News.Add(new T_News
                {
                    Content = model.Content,
                    Type = model.Type,
                    CreateDate = DateTime.Now
                });
                db.SaveChanges();
                TempData[AlertEntity.ALERT_ENTITY] = new AlertEntity(Service.ActionSuccess);
                return RedirectToAction("NewsList");
            }
        }

        public ActionResult NewsList(int pageNum = 1)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                return View(db.News.OrderByDescending(m => m.CreateDate).Page<T_News>(pageNum, pageSize: 2));
            }
        }

        public ActionResult DeleteNews(Guid newsId)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                db.News.Remove(db.News.Find(newsId));
                db.SaveChanges();
                return RedirectToAction("NewsList");
            }
        }

        #endregion

        #region 07. 下拉查询

        public ActionResult Select2()
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var student = db.Student.FirstOrDefault();

                return View(new Select2Data
                {
                    StudentId = student.Id,
                    ClassId = student.ClassId,
                    RoleIdList = string.Join(",", student.StudentRoleLink.Select(m => m.RoleId)) + ","
                });
            }
        }

        [HttpPost]
        public ActionResult Select2(Select2Data model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var student = db.Student.Find(model.StudentId);
                student.ClassId = model.ClassId;

                //删除之前的角色
                db.StudentRoleLink.RemoveRange(student.StudentRoleLink);

                if (!model.RoleIdList.IsNullOrWhiteSpace())
                {
                    foreach (var roleId in model.RoleIdList.Replace("\"", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        db.StudentRoleLink.Add(new T_StudentRoleLink
                        {
                            RoleId = new Guid(roleId),
                            StudentId = student.Id
                        });
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Select2");
            }
        }

        #endregion

        #region 08. 字体图标

        public ActionResult FontIcon()
        {
            return View();
        }

        #endregion

        #region 09. 省市地区

        public ActionResult DisplayPCC()
        {
            return View(new PCCDisplayData { ProvinceId = PCCProvider.DEFAULT_PROVINCE, CityId = PCCProvider.DEFAULT_CITY, CountyId = PCCProvider.DEFAULT_COUNTY });
        }

        [Right(RightType.Anonymous)]
        public JsonResult PCC(int? parentId)
        {
            return Json(PCCProvider.GetPCCItems(parentId), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 10. 弹出窗口
        public ActionResult AlertWindow()
        {
            TempData[AlertEntity.ALERT_ENTITY] = new AlertEntity("output javascript code in section scripts use C#");
            return View();
        }

        #endregion

        #region 11. 格式转换
        public ActionResult FormatConvert()
        {
            return View();
        }

        /// <summary>
        /// 数字转人民币大写
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public JsonResult ConvertToRMB(decimal number)
        {
            var s = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            //if (r.EndsWith("元"))//这两行是我加的
            //    r += "整";//感觉我拉低了前边代码的逼格……很惭愧
            return Json(new ReturnValue { Data = r });
        }

        #endregion

        #region 12. 内存分页
        public ActionResult MemoryPager(int pageSize = 3, int pageNum = 1)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var classList = db.Class.ToList();
                TestPager model = new TestPager()
                {
                    ItemCount = classList.Count,
                    PageNum = pageNum,
                    PageSize = pageSize,
                    ClassList = db.Class.Include(m => m.Student).OrderBy(m => m.Code).Skip(pageSize * (pageNum - 1)).Take(pageSize).ToList()
                };
                return View(model);
            }
        }

        #endregion

        #region 班级管理

        #region 班级列表
        public ActionResult ClassList(string className, DateTime? minDate, DateTime? maxDate, ClassType? classType, int pageNum = 1)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                IQueryable<T_Class> query = db.Class;
                if (string.IsNullOrWhiteSpace(className) == false)
                {
                    //ef 查询支持 Trim() 方法
                    query = query.Where(m => m.Name.Contains(className.Trim()));
                }
                if (minDate.HasValue)
                {
                    query = query.Where(m => m.CreateDate >= minDate.Value);
                }
                if (maxDate.HasValue)
                {
                    query = query.Where(m => m.CreateDate <= maxDate.Value);
                }
                if (classType.HasValue)
                {
                    query = query.Where(m => m.Type == classType.Value);
                }
                return View(query.Include(m => m.Student).OrderBy(m => m.CreateDate).Page<T_Class>(pageNum));
            }
        }

        #endregion

        #region 下载模板
        public FileResult DownLoadClassModel()
        {
            return Download(Server.MapPath(@"~/Upload/Template/班级模板.xls"));
        }

        #endregion

        #region 导入数据
        public ActionResult ImportClassData()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetClassData()
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var file = Request.Files[0];
                var oldName = Path.GetFileName(file.FileName);
                var index = oldName.LastIndexOf(".");
                var path = Server.MapPath(@"~/Upload/Import/" + DateTime.Now.ToString("yyyyMMddHHmmssff") + oldName.Substring(0, index) + Path.GetExtension(oldName));
                file.SaveAs(path);

                if (path != null)
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workBook = null;
                        if (oldName.LastIndexOf(".xls") > 0)
                        {
                            workBook = new HSSFWorkbook(fs);
                        }
                        else if (oldName.LastIndexOf(".xlxs") > 0)
                        {
                            workBook = new XSSFWorkbook(fs);
                        }
                        else
                        {
                            TempData[AlertEntity.ALERT_ENTITY] = new AlertEntity("文件格式有误!", AlertType.Danger);
                            return RedirectToAction("ClassList");
                        }

                        ISheet sheet = workBook.GetSheetAt(0);
                        string errorMessage = string.Empty;
                        //查询所有班级
                        var classCollection = db.Class.ToDictionary(m => m.Code);
                        //保存成功导入的班级记录
                        List<T_Class> classList = new List<T_Class>();
                        //班级类型
                        var classTypeList = ClassType.WeiKe.ToSelectDataSource();

                        for (int i = 1; i < sheet.LastRowNum + 1; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            T_Class @class = new T_Class();
                            if (row != null)
                            {
                                ICell codeCell = row.GetCell(0);
                                if (codeCell == null || string.IsNullOrWhiteSpace(codeCell.StringCellValue.Trim()))
                                {
                                    errorMessage += "第{0}条数据中的班级编码为空！".FormatString(i);
                                    continue;
                                }
                                if (classCollection.ContainsKey(codeCell.StringCellValue.Trim()))
                                {
                                    errorMessage += "第{0}条数据中的班级编码已经存在！".FormatString(i);
                                    continue;
                                }
                                if (Regex.IsMatch(codeCell.StringCellValue.Trim(), @"^\d{3}$") == false)
                                {
                                    errorMessage += "第{0}条数据中的班级编码格式不正确！".FormatString(i);
                                    continue;
                                }
                                @class.Code = codeCell.StringCellValue.Trim();

                                ICell nameCell = row.GetCell(1);
                                if (nameCell == null || string.IsNullOrWhiteSpace(nameCell.StringCellValue.Trim()))
                                {
                                    errorMessage += "第{0}条数据中的班级名称为空！".FormatString(i);
                                    continue;
                                }
                                if (classCollection.Values.Any(m => m.Name == nameCell.StringCellValue.Trim()))
                                {
                                    errorMessage += "第{0}条数据中的班级名称已经存在！".FormatString(i);
                                    continue;
                                }
                                @class.Name = nameCell.StringCellValue.Trim();

                                ICell classTypeCell = row.GetCell(2);
                                if (classTypeCell == null || string.IsNullOrWhiteSpace(classTypeCell.StringCellValue.Trim()))
                                {
                                    errorMessage += "第{0}条数据中的班级类型为空！".FormatString(i);
                                    continue;
                                }
                                if (classTypeList.Any(m => m.Text == classTypeCell.StringCellValue.Trim()) == false)
                                {
                                    errorMessage += "第{0}条数据中的班级类型不正确！".FormatString(i);
                                    continue;
                                }
                                @class.Type = classTypeCell.StringCellValue.Trim().EnumDisplayToEnum<ClassType>();
                                @class.CreateDate = DateTime.Now;
                                classList.Add(@class);
                            }
                        }
                        if (classList.Any())
                        {
                            db.Class.AddRange(classList);
                            db.SaveChanges();
                        }
                        errorMessage += "导入{0}条，成功{1}条".FormatString(sheet.LastRowNum, classList.Count);
                        TempData[AlertEntity.ALERT_ENTITY] = new AlertEntity(errorMessage);
                    }
                }

                return RedirectToAction("ClassList");
            }
        }

        #endregion

        #region 删除班级
        public JsonResult DeleteClass(string classIdList)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                if (classIdList.Length <= 0)
                {
                    return Json(new ReturnValue { IsSuccess = false, Error = "请选择班级" });
                }

                List<T_Class> classList = new List<T_Class>();
                string errorMessage = string.Empty;
                string deletedClassId = string.Empty;
                foreach (var classId in classIdList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var @class = db.Class.Include(m => m.Student).SingleOrDefault(m => m.Id == new Guid(classId));
                    if (@class == null)
                    {
                        continue;
                    }
                    if (@class.Student.Any() == false)
                    {
                        classList.Add(@class);
                        deletedClassId += @class.Id + ",";
                    }
                    else
                    {
                        errorMessage += "{0}下存在学生记录，无法删除；".FormatString(@class.Name);
                    }
                }
                if (classIdList.Any())
                {
                    db.Class.RemoveRange(classList);
                    db.SaveChanges();
                }
                return Json(new ReturnValue { IsSuccess = true, Error = errorMessage + "共计删除{0}条记录".FormatString(classList.Count), Data = deletedClassId });
            }
        }

        #endregion

        #region 编辑班级
        public PartialViewResult EditClass(Guid classId)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var @class = db.Class.Find(classId);
                return PartialView(new EditClassData { ClassId = classId, Name = @class.Name, Code = @class.Code, Type = @class.Type, Description = @class.Description });
            }
        }

        [HttpPost]
        public JsonResult EditClass(EditClassData model)
        {
            if (ModelState.IsValid == false)
            {
                return Json(new ReturnValue { IsSuccess = false, Error = "操作失败！" });
            }

            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var @class = db.Class.Find(model.ClassId);
                @class.Description = model.Description;
                @class.Name = model.Name;
                @class.Type = model.Type;
                @class.Code = model.Code;
                db.SaveChanges();
                return Json(new ReturnValue { IsSuccess = true, Error = "操作成功！" });
            }
        }

        #endregion

        #region 查询编码
        public JsonResult QueryClassCode(string code, Guid classId)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var @class = db.Class.FirstOrDefault(m => m.Code == code.Trim() && m.Id != classId);
                if (@class != null)
                {
                    return Json(new ReturnValue { IsSuccess = false, Error = "班级编码已经存在！" });
                }
                return Json(new ReturnValue { IsSuccess = true });
            }
        }

        #endregion

        #region 导出数据
        public void DownLoadClassData()
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                IWorkbook workBook = new HSSFWorkbook();
                ISheet sheet = workBook.CreateSheet("班级信息表");
                IRow headingRow = sheet.CreateRow(0);
                headingRow.CreateCell(0).SetCellValue("编码");
                headingRow.CreateCell(1).SetCellValue("名称");
                headingRow.CreateCell(2).SetCellValue("类型");
                headingRow.CreateCell(3).SetCellValue("班级人数");
                headingRow.CreateCell(4).SetCellValue("创建日期");

                var classList = db.Class.ToList();
                for (int i = 0; i < classList.Count; i++)
                {
                    IRow classRow = sheet.CreateRow(i + 1);
                    classRow.CreateCell(0).SetCellValue(classList[i].Code);
                    classRow.CreateCell(1).SetCellValue(classList[i].Name);
                    classRow.CreateCell(2).SetCellValue(classList[i].Type.EnumMetadataDisplay());
                    classRow.CreateCell(3).SetCellValue(classList[i].Student.Count);
                    classRow.CreateCell(4).SetCellValue(classList[i].CreateDate.ToString("yyyy-MM-dd"));
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "UTF-8";
                Response.ClearHeaders();
                string outputFileName = "班级信息表.xls";

                if (Request.UserAgent.ToLower().Contains("firefox") == false)
                {
                    outputFileName = Url.Encode(outputFileName);
                }

                Response.AddHeader("Content-Disposition", "attachment;filename=" + outputFileName);
                Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
                Response.ContentType = "application/ms-excel";

                using (MemoryStream ms = new MemoryStream())
                {
                    workBook.Write(ms);
                    Response.BinaryWrite(ms.GetBuffer());
                    Response.End();
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region 功能模块二

        #region 01. 页面操作

        public ActionResult OperatePage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OperatePage(OperatePageData model)
        {
            var dropdown = JsonConvert.DeserializeObject<StoreData[]>(model.DropdownListTest).ToList();
            var selectoption = model.SelectOptionTest.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var inputgroup = JsonConvert.DeserializeObject<StoreData[]>(model.InputGroupTest).ToList();
            return RedirectToAction("OperatePage");
        }

        #endregion

        #region 02. 图片保存

        public ActionResult SaveImgage()
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var model = new SaveImgageData { };
                model.ImageDataList.AddRange(db.Image.OrderBy(m => m.CreateDate).Select(m => new ImageData { Url = m.Url, ImgageByte = m.ImageByte }));
                model.AddImageList = string.Join(",,,", model.ImageDataList.Select(m => m.Url)) + ",,,";
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult SaveImgage(SaveImgageData model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            if (!model.AddImageList.IsNullOrWhiteSpace())
            {
                using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
                {
                    //原有图片
                    var oldImageList = db.Image.OrderBy(m => m.CreateDate).ToList();
                    List<T_Image> checkImageList = new List<T_Image>();

                    //重新保存图片
                    foreach (var url in model.AddImageList.Split(new string[] { ",,," }, StringSplitOptions.RemoveEmptyEntries).Where(m => m.Length >= 20))
                    {
                        //判断是否删除原有图片
                        if (oldImageList.Any(m => m.Url == url))
                        {
                            checkImageList.Add(oldImageList.FirstOrDefault(m => m.Url == url));
                            continue;
                        }

                        //获取图片原名
                        var fileName = System.IO.Path.GetFileName(url).GetOriginalName();
                        db.Image.Add(new T_Image
                        {
                            Name = fileName,
                            //保存图片相对路径
                            Url = url,
                            //保存图片二进制形式
                            ImageByte = Server.MapPath(url).ConvertImgToBase64(),
                            CreateDate = DateTime.Now
                        });
                    }

                    //要删除的图片
                    var deleteImageList = oldImageList.Except(checkImageList).ToList();
                    //删除原有图片
                    foreach (var item in deleteImageList)
                    {
                        if (System.IO.File.Exists(Server.MapPath(item.Url)))
                        {
                            System.IO.File.Delete(Server.MapPath(item.Url));
                        }
                    }
                    db.Image.RemoveRange(deleteImageList);

                    db.SaveChanges();
                    TempData[AlertEntity.ALERT_ENTITY] = new AlertEntity(Service.ActionSuccess);
                }
            }

            return RedirectToAction("SaveImgage");
        }

        #endregion

        #region 03. 自定义列

        public ActionResult CustomizeColumnList(int pageNum = 1)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                return View(db.Student.Select(m => new StudentListData
                {
                    StudentId = m.Id,
                    Name = m.Name,
                    Sex = m.Sex,
                    Code = m.Code,
                    ClassName = m.Class.Name,
                    Birthday = m.Birthday,
                    Age = m.Age,
                    Mobile = m.Mobile,
                    IsZhuXiao = m.IsZhuXiao
                }).OrderBy(m => m.Code).Page<StudentListData>(pageNum));
            }
        }

        #endregion

        #region 04. 颜色控件

        public ActionResult SelectColor()
        {
            return View(new SelectColorData { Color1 = "#fdbace", Color2 = "#01efda" });
        }

        #endregion

        #endregion

        #region 插件使用

        #region BootstrapFileInput

        public ActionResult BootstrapFileInput()
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var test = db.Test.FirstOrDefault();
                ViewBag.TestId = test.Id;
                var model = new List<UploadFileData>();
                if (string.IsNullOrWhiteSpace(test.ImgUrl) == false)
                {
                    model.AddRange(test.ImgUrl.Split(new string[] { @"///" }, StringSplitOptions.RemoveEmptyEntries).Select(m => new UploadFileData
                    {
                        Key = m,
                        Caption = Path.GetFileNameWithoutExtension(m).GetOriginalName() + Path.GetExtension(m),
                        Size = (new FileInfo(Server.MapPath(m))).Length,
                        Type = Path.GetExtension(m).GetFileType(),
                        Width = "100px",
                        Height = "115px"
                    }));
                }
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult BootstrapFileInput(Guid testId, string imgUrlList)
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var test = db.Test.Find(testId);
                List<string> oldImgUrlList = new List<string>(), newImgUrlList = new List<string>();
                if (string.IsNullOrWhiteSpace(test.ImgUrl) == false)
                {
                    oldImgUrlList.AddRange(test.ImgUrl.Split(new string[] { "///" }, StringSplitOptions.RemoveEmptyEntries).ToList());
                }
                if (string.IsNullOrWhiteSpace(imgUrlList) == false)
                {
                    newImgUrlList.AddRange(imgUrlList.Split(new string[] { "///" }, StringSplitOptions.RemoveEmptyEntries).ToList());
                }
                //删除原有文件
                foreach (var item in oldImgUrlList.Except(newImgUrlList))
                {
                    var filePath = Server.MapPath(@"~{0}".FormatString(item));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                test.ImgUrl = imgUrlList;
                db.SaveChanges();
                return RedirectToAction("BootstrapFileInput");
            }
        }

        [AllowAnonymous]
        public JsonResult FileUpload()
        {
            //var testId = Request.Params["testId"]; //获取上传的附加参数

            var returnJson = new
            {
                initialPreview = new List<string>(),
                initialPreviewConfig = new List<object>(),
                append = true
            };
            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var item = Request.Files[i];
                    var itemFileName = Path.GetFileNameWithoutExtension(item.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss") + PF.Key().ToString().Substring(3, 5) + Path.GetExtension(item.FileName);
                    var itemFilePath = Server.MapPath(@"~/Upload/Test/{0}".FormatString(itemFileName));
                    if (System.IO.File.Exists(itemFilePath))
                    {
                        System.IO.File.Delete(itemFilePath);
                    }
                    item.SaveAs(itemFilePath);
                    var path = "/" + itemFilePath.Substring(AppDomain.CurrentDomain.BaseDirectory.Length).Replace(@"\", @"/");
                    var type = "other";
                    type = item.ContentType.Contains("image") ? "image" : type;
                    type = item.ContentType.Contains("video") ? "video" : type;
                    returnJson.initialPreview.Add(path);
                    returnJson.initialPreviewConfig.Add(new
                    {
                        type = type,
                        caption = item.FileName,
                        size = item.ContentLength,
                        width = "100px",
                        frameAttr = new
                        {
                            style = "height:115px"
                        },
                        url = "/System/DeleteFileUpload",
                        key = path
                    });
                }
            }
            return Json(returnJson, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult DeleteFileUpload(string key)
        {
            // 根据 testId 是否为空判断是否删除文件
            if (string.IsNullOrWhiteSpace(Request.Params["testId"]))
            {
                var filePath = Server.MapPath(@"~{0}".FormatString(key));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region 测试

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult Test2()
        {
            return View();
        }

        public ActionResult Test3()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Test(TestData model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            return RedirectToAction("Test");
        }

        [HttpPost]
        public JsonResult ProcessStudent()
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                var studentList = db.Student.ToList();
                return Json(new
                {
                    IsSuccess = true,
                    StudentData = studentList.Select(m => new SerializeStudentData { Name = m.Name, Code = m.Code })
                });
            }
        }

        public ActionResult DeleteAuthor()
        {
            using (KeKeSoftPlatformDbContext db = new KeKeSoftPlatformDbContext())
            {
                db.Author.RemoveRange(db.Author);
                db.SaveChanges();
                return RedirectToAction("test");
            }
        }

        [Right(RightType.Anonymous)]
        public ActionResult Unauthorized()
        {
            return View();
        }

        #endregion
    }
}