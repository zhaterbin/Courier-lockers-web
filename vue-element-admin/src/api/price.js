import request from '@/utils/request'


export function  fetchReportList(data) {
        return request({
          url: `/api/InStorage/PriceRulerPage`,
          method: 'post',
          headers: {
            'Content-Type': 'application/json'
          },
          data
        })
    }
