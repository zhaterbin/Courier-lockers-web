namespace Courier_lockers.Models
{
    public class Enum
    {

        public enum Cabinet
        {
            opening,
            closing
        }
        /// <summary>
        /// 快递箱大小
        /// </summary>
        public enum SHELF_TYPE
        {
            /// <summary>
            /// 大
            /// </summary>
            Sgoods,
            /// <summary>
            /// 中
            /// </summary>
            Mgoods,
            /// <summary>
            /// 小
            /// </summary>
            Bgoods
        }
        /// <summary>
        /// 运行状态
        /// </summary>
        public enum RUN_STATUS_ENMU
        {
            /// <summary>
            /// 禁用
            /// </summary>
            Disable,
            /// <summary>
            /// 待用
            /// </summary>
            Enable,
            /// <summary>
            /// 运行
            /// </summary>
            Run
        }

        /// <summary>
        /// 存储状态
        /// </summary>
        public enum CELL_STATUS_ENUM
        {
            /// <summary>
            /// 有
            /// </summary>
            Full,
            /// <summary>
            /// 无
            /// </summary>
            Nohave,

            /// <summary>
            /// 异常
            /// </summary>
            Exception,

            /// <summary>
            /// 禁用
            /// </summary>
            Forbiden
        }
    }
}
