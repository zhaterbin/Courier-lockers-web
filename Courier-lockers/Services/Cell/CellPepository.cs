using Courier_lockers.Data;
using Courier_lockers.Entities;
using Courier_lockers.Repos.Cell;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using static Courier_lockers.Models.Enum;

namespace Courier_lockers.Services.Cell
{
    public class CellPepository : ICellRepository
    {
        private readonly ServiceDbContext _context;
        public CellPepository(ServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ActionResult<CellReqReturn> GetCellArea(CellRequst cellRequst)
        {
            CellReqReturn cellReqReturn = new ();
            var kvPairs = _context.Cells.Where(f => f.CELL_X == cellRequst.AreaCode).Select(v => new CellX_Y_Z
            {
                Cell_Type = v.CELL_TYPE,
                Cell_Status = v.CELL_STATUS,
                Run_Status = v.RUN_STATUS,
                Shelf_Type = v.SHELF_TYPE,
                Cell_Code = v.CELL_CODE,
                Cell_X = v.CELL_X,
                Cell_Y = v.CELL_Y,
                Cell_Z = v.CELL_Z
            })
            .ToList().GroupBy(x=>x.Cell_X)
            .ToDictionary(
                k => k.Key,
                v => v.ToList()
            );
            foreach (var item in kvPairs)
            {
                cellReqReturn.Area = item.Key;
                cellReqReturn.GetX_Y_Zs = item.Value;
            }
            return cellReqReturn;

        }
        /// <summary>
        /// 别动，我以后优化
        /// </summary>
        /// <param name="idex"></param>
        /// <param name="Sheft"></param>
        /// <returns></returns>
        public async Task<int> GetCellCodeId(string idex, string Sheft)
        {

            var cell = await _context.Cells.Where(f => f.CELL_TYPE == "Normal" && f.CELL_STATUS == "Nohave" && f.RUN_STATUS == "Enable" && f.SHELF_TYPE == Sheft
            && f.CELL_X == idex && f.Cabinet == "closing")
                .OrderBy(f => f.CELL_Z).ThenBy(f => f.CELL_Y).FirstOrDefaultAsync();

            if (cell == null)
            {
                var cellStation = await _context.Cells.Where(f => f.CELL_TYPE == "Normal" && f.CELL_STATUS == "Nohave" && f.RUN_STATUS == "Enable" && f.SHELF_TYPE == Sheft
                                    && f.CELL_X == idex && f.Cabinet == Cabinet.opening.ToString())
                                        .OrderBy(f => f.CELL_Z).ThenBy(f => f.CELL_Y).FirstOrDefaultAsync();
                return cellStation != null ? cellStation.CELL_ID : 0;
            }
            return cell != null ? cell.CELL_ID : 0;
        }

        public async Task OutStatus(int cELL_ID)
        {
            var cell=await _context.Cells.Where(f=>f.CELL_ID==cELL_ID).FirstOrDefaultAsync();
            if (cell != null)
            {
                cell.CELL_STATUS = RUN_STATUS_ENMU.Enable.ToString();
                cell.CELL_TYPE = CELL_STATUS_ENUM.Nohave.ToString();
                _context.Cells.Update(cell);

                _context.SaveChanges();
                
            }
          
        }

        public async Task updateStatus(int ite)
        {
            try
            {
                var cell=await _context.Cells.Where(f=>f.CELL_ID==ite).FirstOrDefaultAsync();
                if (cell != null) 
                {
                    cell.RUN_STATUS = RUN_STATUS_ENMU.Run.ToString();
                    cell.CELL_STATUS = CELL_STATUS_ENUM.Full.ToString();
                    cell.Cabinet=Cabinet.opening.ToString();
                    _context.Cells.Update(cell);
                    _context.SaveChanges();
                }
            }
            catch
            {

            }
    }

        public async  Task<bool> UpTabaleXYZ()
        {
           var st= await _context.Cells.Where(f => true).ToListAsync();

            foreach (var item in st)
            {
                var strings=item.CELL_CODE.Split("-");
                item.CELL_X = strings[0];
                item.CELL_Y = strings[1];
                item.CELL_Z = strings[2];
                _context.Update(item);
            }
            return await _context.SaveChangesAsync()>0;
        }
    }
}
