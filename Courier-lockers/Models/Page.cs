using System.Collections.Generic;

namespace WMSService.Models
{
    public class Page<T>
    {
        private int _current = 1;
        private int _pageSize = 15;
        public Page()
        {
        }
        public Page(int pageSize, int total, int currentPage, List<T> data)
        {
            this.pageSize = pageSize;
            this.total = total;
            current = currentPage;
            this.data = data;
        }
        /// <summary>
        /// 页大小
        /// </summary>
        public int pageSize
        {
            get => _pageSize;
            set
            {
                if (value <= 0) value = 15;
                _pageSize = value;
            }
        }
        /// <summary>
        /// 总条数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 当前第几页
        /// </summary>
        public int current 
        {
            get => _current;
            set
            {
                if (value <= 0) value = 1;
                _current = value;
            }
        }
        /// <summary>
        /// 请求数据
        /// </summary>
        public T requestData { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> data { get; set; } = new();
    }
}
