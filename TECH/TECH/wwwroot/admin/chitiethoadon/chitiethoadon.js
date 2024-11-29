(function ($) {
    var self = this;
    self.Data = [];
    self.UserImages = {};
    self.IsUpdate = false;   
    self.HoaDon = {
        Id: 0,
        MaPhongs: [],
        TrangThai: "",
        HanDong: null,
        GhiChu:""
    }
    self.ChiTietHoaDon = {
        Id: 0,
        MaHoaDon:0,
        MaDV:0,
        ChiSoCu:0,
        ChiSoMoi:0,
        ChiSoDung:0
    }
    self.UserSearch = {
        name: "",
        role: null,
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }
    self.lstRole = [];

    self.addSerialNumber = function () {
        var index = 0;
        $("table tbody tr").each(function (index) {
            $(this).find('td:nth-child(1)').html(index + 1);
        });
    };

    self.GetDichVuPhong = function (maDichVu,maPhong,maHoaDon,tag) {
        if (maDichVu != null && maDichVu != "" && maPhong != null && maPhong != "") {
            $.ajax({
                url: '/Admin/ChiTietHoaDon/ViewDichVu',
                type: 'GET',
                dataType: 'html',
                //contentType: 'application/html; charset=utf-8', 
                data: {
                    maDichVu: maDichVu,
                    maPhong: maPhong,
                    maHoaDon: maHoaDon
                },
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (data) {
                    if (data != null) {
                        $(".content-dich-vu").html(data);
                    }
                    $(".custom-item").removeClass('active');
                    $(tag).addClass('active');
                    /*console.log(data);*/
                    //if (response.Data != null) {
                    //    renderCallBack(response.Data);
                    //    self.Id = id;

                    //}
                }
            })
        }
    }
    self.SaveDichVuThanhToan = function (maHoaDon,maDichVu) {
        if (maHoaDon != null && maHoaDon != "" && maDichVu != null && maDichVu != "") {
            self.ChiTietHoaDon.MaDV = maDichVu;
            self.ChiTietHoaDon.MaHoaDon = maHoaDon;
            var chisocu = $("." + maDichVu.toString() + " .chisocu").val();
            if (chisocu != "") {
                chisocu = parseInt(chisocu);
                self.ChiTietHoaDon.ChiSoCu = chisocu;
            }

            var chisomoi = $("." + maDichVu.toString() + " .chisomoi").val();
            if (chisomoi != "") {
                chisomoi = parseInt(chisomoi);
                self.ChiTietHoaDon.ChiSoMoi = chisomoi;
            }

            var chisodung = $("." + maDichVu.toString() + " .chisodung").val();
            if (chisodung != "") {
                chisodung = parseInt(chisodung);
                self.ChiTietHoaDon.ChiSoDung = chisodung;
            }
            $.ajax({
                url: '/Admin/ChiTietHoaDon/SaveDichVuThanhToan',
                type: 'POST',
                dataType: 'json',
                data: {
                    chiTietHoaDonModelView: self.ChiTietHoaDon
                },
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (response) {
                    if (response.success) {
                        tedu.notify('Cập nhật dữ liệu thành công', 'success');
                        window.location.reload();
                    }                   
                }
            })
        }
    }
    // Thêm mã lỗi vi phạm

    $(document).ready(function () {     
        $('.tabcustom > li').each(function (index, tr) {
            if (index == 0) {
                $(tr).find('a').click();
            }

        })
    })
})(jQuery);