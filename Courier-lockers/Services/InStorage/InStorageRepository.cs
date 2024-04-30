using Courier_lockers.Data;
using Courier_lockers.Entities;
using Courier_lockers.Helper;
using Courier_lockers.Repos;
using Courier_lockers.Repos.Cell;
using Courier_lockers.Repos.InStorage;
using Courier_lockers.Services.Cell;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using System.Text.RegularExpressions;
using static ServiceStack.Script.Lisp;

namespace Courier_lockers.Services.InStorage
{
    public class InStorageRepository : IInStorageRepository
    {
        readonly string pattern = @"^\d{4}$";
        private readonly ServiceDbContext _context;
        private readonly ICellRepository _cellRepository;
        private readonly IWPFHttpClient _WpfHttp;
        public InStorageRepository(ServiceDbContext context, ICellRepository cellRepository,IWPFHttpClient wPFHttpClient)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cellRepository = cellRepository ?? throw new ArgumentNullException(nameof(cellRepository));
            _WpfHttp = wPFHttpClient ?? throw new ArgumentNullException(nameof(cellRepository));
        }

        public async Task<Result> EntryStorage(ReqStorage reqStorage)
        {
            Result result = new Result() { Success = true, Messsage = string.Empty };
            var db = _context.Database.BeginTransaction();
            var bselt = false;
            try
            {
                Regex regex = new Regex(pattern);
                if (!regex.IsMatch(reqStorage.incode))
                {
                    result.Success = false;
                    result.Messsage = "输入单号不是4位数";
                    return result;
                }
                var ite=await _cellRepository.GetCellCodeId(reqStorage.idex,reqStorage.ShelfType);
                if (ite == 0)
                {
                    result.Success = false;
                    result.Messsage = "此排没有空的快递架了";
                    return result;
                }
                var storage=await _context.storages.Where(f => f.InCode == reqStorage.incode).FirstOrDefaultAsync();
                if (storage != null)
                {
                    result.Success = false;
                    result.Messsage = "已有此单";
                    return result;
                }
                _context.storages.Add(new Storage
                {
                    CELL_ID= ite,
                    Bar_Code="Test",
                    Bar_Name="Test",
                    Entry_Time =DateTime.Now.ToString(),
                    Storage_Name="WPF国手",
                    InCode=reqStorage.incode
                });
                await _context.SaveChangesAsync();
                _context.opearterIns.Add(new OpearterIn {
                    Operator_Name = "S神",
                    Price=1,
                    Storage_ID= _context.storages.OrderByDescending(f=>f.STORAGE_ID).FirstOrDefault().STORAGE_ID,
                    Storage_Name="WPF国手",
                    DateTime= DateTime.Now.ToString(),
                    InCode= reqStorage.incode,
                });
                await _context.SaveChangesAsync();
                await _cellRepository.updateStatus(ite);
                result.Messsage = "送入快递柜成功";
                db.Commit();
                //完成后下放给3d动作 快递柜打开
               var cellcode=_context.Cells.SingleOrDefault(f => f.CELL_ID == ite);
                //if (cell_code != null)
                //{
                //    RequestUnity requestUnity = new RequestUnity
                //    {
                //        code = cell_code.CELL_CODE,
                //        IsOpening = cell_code.Cabinet
                //    };
                //    var s=_WpfHttp.PostData("Wpf", requestUnity);
                //}
                result.Success = true;
                result.Messsage = "送入快递柜成功";
                result.data = new Repos.DataBase
                {
                    cell_code= cellcode.CELL_CODE,
                    Isopening= "closing"
                };
            }
            catch (Exception ex)
            {
                bselt= true;
                result.Messsage = "送入快递柜失败";
                await Console.Out.WriteLineAsync(ex.Message);
            }
            finally
            {
                if (bselt)
                {
                    db.Rollback();
                }

            }
            return result;
        }

        public async  Task deStorage(Storage storage)
        {
            var opearterIn=await _context.opearterIns.Where(f => f.InCode == storage.InCode).FirstOrDefaultAsync();
            if (opearterIn != null)
            {
                _context.storages.Remove(storage);
                _context.opearterIns.RemoveRange(opearterIn);
                await _context.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 时间对应金额待完成 
        /// </summary>
        /// <param name="reqOutStorage"></param>
        /// <returns></returns>
        public async  Task<Result> OutStorage(ReqOutStorage reqOutStorage)
        {
            Result result = new Result() { Success = true, Messsage = string.Empty };
            var db = _context.Database.BeginTransaction();
            var bselt = false;
            try
            {
                Regex regex = new Regex(pattern);
                if (!regex.IsMatch(reqOutStorage.incode))
                {
                    result.Success = false;
                    result.Messsage = "输入单号不是4位数";
                    return result;
                }
                var storage=await _context.storages.Where(f => f.InCode == reqOutStorage.incode).FirstOrDefaultAsync();
                if (storage==null)
                {
                    result.Success = false;
                    result.Messsage = "没有此单";
                    return result;
                }

                await _cellRepository.OutStatus(storage.CELL_ID);
                
                await deStorage(storage);
                db.Commit();

            }
            catch  (Exception ex)
            {
                bselt = true;
                result.Messsage = "";
                await Console.Out.WriteLineAsync(ex.Message);
            }
            finally
            {
                if (bselt)
                {
                    db.Rollback();
                }
                result.Success = true;
                result.Messsage = "送入快递柜成功";
            }
            return result;
        }
    }
}
