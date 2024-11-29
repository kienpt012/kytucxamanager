using Microsoft.CodeAnalysis.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TECH.General
{
   public class General
    {
        public enum StaffStatus
        {
            Active = 1, // đang làm việc
            InActive = 2 // nghỉ làm việc
        }
        public enum OrdersStatus
        {
            Delivered = 1, // đã giao hàng
            Cancel = 2 // Trả lại hàng
        }
        public enum ProductStatus
        {
            Show = 1, // Sản phẩm hót
            Hide = 2, // Trả lại hàng
            Wait = 3
        }
    }
    public static class Common
    {
        public static string GetTrademark(int trademark)
        {
            if (trademark == 1)
            {
                return "Adidas";
            }
            if (trademark == 2)
            {
                return "Nike";
            }
            if (trademark == 3)
            {
                return "MLB Korea";
            }
            if (trademark == 4)
            {
                return "New Balance";
            }
            if (trademark == 5)
            {
                return "McQueen";
            }
            if (trademark == 6)
            {
                return "Converse";
            }
            return "Thương Hiệu khác";
        }
        public static string GetColor(int colorId)
        {
            if (colorId == 1)
            {
                return "Vàng đồng";
            }
            if (colorId == 2)
            {
                return "Trắng";
            }
            if (colorId == 3)
            {
                return "Đen";
            }
            if (colorId == 4)
            {
                return "Trắng bạc";
            }            
            return "Màu khác";
        }

        public static string GetDichVu(int trademark)
        {            

            if (trademark == 0)
            {
                return "Điện";
            }
            if (trademark == 1)
            {
                return "Nước";
            }
            if (trademark == 2)
            {
                return "Rác";
            }
            if (trademark == 3)
            {
                return "Mạng";
            }
            if (trademark == 4)
            {
                return "Trông xe";
            }
            return "Dịch vụ khác";
        }

        public static string GetStatusSuaChua(int status)
        {
            
            if (status == 1)
            {
                return "Chưa sửa";
            }
            if (status == 2)
            {
                return "Đang sửa";
            }
            if (status == 3)
            {
                return "Đã sửa";
            }            
            return "";
        }



        public static string GetStatusStr(int staus)
        {
            if (staus == 1)
            {
                return "Hàng mới";
            }
            if (staus == 2)
            {
                return "Nổi bật";
            }
            return "";
        }
        public static string GetLoaiPhong(int loaiphong)
        {
            if (loaiphong == 2)
            {
                return "Nam";
            }
            if (loaiphong == 3)
            {
                return "Nữ";
            }            
            return "Cả nam và nữ";
        }
        public static int GetLoaiPhongForText(string loaiphong)
        {
            if (loaiphong.ToLower() == "nam")
            {
                return 2;
            }
            if (loaiphong.ToLower() == "nữ")
            {
                return 3;
            }
            if (loaiphong.ToLower() == "Cả nam và nữ".ToLower())
            {
                return 1;
            }

            return 0;
        }
        public static string GetTinhTrangPhong(int tinhtrangphong)
        {
            if (tinhtrangphong == 1)
            {
                return "Trống";
            }
            if (tinhtrangphong == 2)
            {
                return "Đã thuê";
            }
            //if (tinhtrangphong == 3)
            //{
            //    return "Đã thuê";
            //}
            return " Hết chỗ";
        }
        public static int GetTinhTrangPhongForText(string tinhtrangphong)
        {
            if (tinhtrangphong.ToLower() == "trống")
            {
                return 1;
            }
            if (tinhtrangphong.ToLower() == "đã thuê")
            {
                return 2;
            }
            if (tinhtrangphong.ToLower() == "Hết chỗ".ToLower())
            {
                return 3;
            }
            return 0;
        }
        public static string GetTinhTrangHoaDon(int tinhtrangphong)
        {
            if (tinhtrangphong == 1)
            {
                return "Đã thuê";
            }
            //if (tinhtrangphong == 2)
            //{
            //    return "Chờ đến ở";
            //}
            if (tinhtrangphong == 2)
            {
                return "Hủy thuê";
            }            
            return "";
        }
        public static string GetTrangThaiHoaDon(int tinhtrangphong)
        {
            if (tinhtrangphong == 1)
            {
                return "Đã đóng";
            }
            if (tinhtrangphong == 2)
            {
                return "Chưa đóng";
            }
            //if (tinhtrangphong == 3)
            //{
            //    return "Quá hạn";
            //}
            if (tinhtrangphong == 3)
            {
                return "Còn nợ";
            }
            return "";
        }        
    }
}
