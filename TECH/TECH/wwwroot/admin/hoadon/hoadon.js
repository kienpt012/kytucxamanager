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
    self.UserSearch = {
        name: "",
        status: 0,
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
    self.Files = {};

    self.RenderTableHtml = function (data) {
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                html += "<tr>";
                html += "<td>" + (++index) + "</td>";
                //html += "<td>" + item.Id + "</td>";
                /*html += "<td>" + item.MaHopDong + "</td>";*/
                if (item.HopDong != null && item.HopDong.KhachHang != null) {
                    html += "<td>" + item.HopDong.KhachHang.TenKH + "</td>";
                }
                else {
                    html += "<td></td>";
                }

                if (item.HopDong != null && item.HopDong.Phong != null) {
                    html += "<td>" + item.HopDong.Phong.TenPhong + "</td>";
                }
                else {
                    html += "<td></td>";
                }
                
                html += "<td>" + item.NguoiDong + "</td>";
                html += "<td>" + item.NgayDongTienStr + "</td>";
                html += "<td>" + item.HanDongStr + "</td>";
                html += "<td>" + item.TongTienStr + "</td>";
                html += "<td>" + item.TienDongStr + "</td>";
                html += "<td>" + item.TrangThaiStr + "</td>";  
                html += "<td style=\"text-align: center;\">" +
                    "<button  class=\"btn btn-primary custom-button\" onClick=\"PayBill(" + item.Id + "," + item.HopDong.Phong.Id + ")\"><i  class=\"fa-solid fa-file-invoice-dollar\"></i></button>" +
                    "<button  class=\"btn btn-primary custom-button\" onClick=\"Update(" + item.Id + "," + item.HopDong.Phong.Id + ")\"><i  class=\"fa-regular fa-rectangle-list\"></i></button>" +
                    
                    "<button  class=\"btn btn-danger custom-button\" onClick=\"Deleted(" + item.Id +")\"><i  class=\"bi bi-trash\"></i></button>" +
                    "</td>";                
                html += "</tr>";
            }
        }
        else {
            html += "<tr><td colspan=\"11\" style=\"text-align:center\">Không có dữ liệu</td></tr>";
        }
        $("#tblData").html(html);
    };
    self.PayBill = function (maHopDong, maPhong) {
        self.GetHoaDonId(maHopDong, maPhong);
       
    }

    self.GetHoaDonId = function (maHopDong, maPhong) {
        if (maHopDong != null && maHopDong != "") {
            $.ajax({
                url: '/Admin/HoaDon/GetHoaDonByMaHoaDonMaPhong',
                type: 'GET',
                dataType: 'json',
                data: {
                    mahoadon: maHopDong,
                    maphong: maPhong
                },
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (response) {
                    if (response.Data != null) {
                        var item = response.Data;
                        if (item.TrangThai != 1) {
                            $(".custom-disble").attr("disabled", false);
                            $("#mahoadonhidden").val(maHopDong);
                            $("#tongtienhidden").val(item.TongTien);
                            
                        } else {
                            $(".custom-disble").attr("disabled", true);
                        }
                        $("#TongTien").val(item.TongTienStr);
                        $("#TienDong").val(item.TienDong);
                        $("#NguoiDong").val(item.NguoiDong);
                        $("#GhiChu").val(item.GhiChu);
                        $("#TrangThaiBill").val(item.TrangThai);
                        $('#paybill').modal('show');
                    }
                }
            })
        }
    }
    self.Update = function (id,maphong) {
        if (id != null && id != "") {
            window.location.href = '/Admin/ChiTietHoaDon/Index?mahoadon=' + id+ '&maphong=' + maphong;
        }
    }

    self.GetById = function (id, renderCallBack) {
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/HoaDon/GetById',
                type: 'GET',
                dataType: 'json',
                data: {
                    id: id
                },
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (response) {
                    if (response.Data != null) {
                        renderCallBack(response.Data);
                        self.Id = id;
                        
                    }
                }
            })
        }
    }

    self.GetAllNha = function () {
        $.ajax({
            url: '/Admin/Nha/GetAll',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var html = "<option value =\"\">Chọn nhà</option>";
                if (response.Data != null && response.Data.length > 0) {
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.Id + ">" + item.TenNha + "</option>";
                    }
                }
                $("#MaNha").html(html);
            }
        })
    }

    self.GetAllPhong = function () {
        $.ajax({
            url: '/Admin/HoaDon/GetPhongByHopDong',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {                
                if (response.Data != null && response.Data.length > 0) {
                    var html = "<option value ='0'>Tất cả</option>";
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.Id + ">" + item.TenPhong + "</option>";
                    }
                    $("#MaPhong").html(html);
                }               
            }
        })
    }
    self.GetAllKhachHang = function () {
        $.ajax({
            url: '/Admin/KhachHang/GetAll',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var html = "<option value =\"\">Chọn khách hàng</option>";
                if (response.Data != null && response.Data.length > 0) {
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.Id + ">" + item.TenKH + "</option>";
                    }
                }
                $("#MaKH").html(html);
            }
        })
    }
    self.GetAllNhanVien = function () {
        $.ajax({
            url: '/Admin/NhanVien/GetAll',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var html = "<option value =\"\">Chọn nhân viên</option>";
                if (response.Data != null && response.Data.length > 0) {
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.Id + ">" + item.TenNV + "</option>";
                    }
                }
                $("#MaNV").html(html);
            }
        })
    }

    self.WrapPaging = function (recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / tedu.configs.pageSize);
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: '<<',
            prev: '<',
            next: '>',
            last: '>>',
            onPageClick: function (event, p) {
                tedu.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }

    self.Deleted = function (id) {
        if (id != null && id != "") {
            tedu.confirm('Bạn có chắc muốn xóa hóa đơn này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/HoaDon/Delete",
                    data: { id: id },
                    beforeSend: function () {
                    },
                    success: function () {
                        tedu.notify('Đã xóa thành công', 'success');
                        window.location.reload();
                        /*self.GetDataPaging(true);*/
                    },
                    error: function () {
                        tedu.notify('Has an error', 'error');
                    }
                });
            });
        }
    }

    self.GetDataPaging = function (isPageChanged) {
        var _data = {
            Name: $(".name-search").val() != "" ? $(".name-search").val() : null,
            Code: $(".code-search").val() != "" ? $(".code-search").val() : null,
            PageIndex: tedu.configs.pageIndex,
            PageSize: tedu.configs.pageSize
        };

        self.UserSearch.PageIndex = tedu.configs.pageIndex;
        self.UserSearch.PageSize = tedu.configs.pageSize;

        $.ajax({
            url: '/Admin/HoaDon/GetAllPaging',
            type: 'GET',
            data: self.UserSearch,
            dataType: 'json',
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                self.RenderTableHtml(response.data.Results);
                $('#lblTotalRecords').text(response.data.RowCount);
                if (response.data.RowCount != null && response.data.RowCount > 0) {
                    self.WrapPaging(response.data.RowCount, function () {
                        GetDataPaging();
                    }, isPageChanged);
                }

            }
        })
    };

    self.Init = function () {
        $(".btn-add").click(function () {
            self.SetValueDefault();
            self.KhachHang.Id = 0;
            $('#CreateOrUpdate').modal('show')
        })

        // hủy add và edit
        $(".cs-close-addedit,.btn-cancel-addedit").click(function () {
            $("#CreateEdit").css("display", "none");
        })


        $('body').on('click', '.btn-edit', function () {
            $(".user .modal-title").text("Chỉnh sửa thông tin người dùng");
            var id = $(this).attr('data-id');
            if (id !== null && id !== undefined) {
                self.GetUserById(id);
                $('#create').modal('show');
            }
        })

        $('body').on('click', '.btn-delete', function () {
            var id = $(this).attr('data-id');
            var fullname = $(this).attr('data-fullname');
            if (id !== null && id !== '') {
                self.confirmUser(fullname, id);
            }
        })
    }

    self.AddUser = function (userView) {
        $.ajax({
            url: '/Admin/HoaDon/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                HoaDonModelView: userView
            },
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    tedu.notify('Thêm mới dữ liệu thành công', 'success');
                    /*self.GetDataPaging(true);*/
                    $('#userModal').modal('hide');
                    window.location.reload();
                }
                else {
                    if (response.isPhoneExist) {
                        $(".phone_number-exist").show().text("Phone đã tồn tại");
                    }
                    if (response.isMailExist) {
                        $(".email-exist").show().text("Email đã tồn tại");
                    }
                }
            }
        })
    }


    self.UpdateUser = function (userView) {
        $.ajax({
            url: '/Admin/HoaDon/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                HoaDonModelView: userView
            },
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    tedu.notify('Cập nhật dữ liệu thành công', 'success');
                    /*self.GetDataPaging(true);*/
                    $('#userModal').modal('hide');
                    window.location.reload();
                }
               
            }
        })
    }

    self.ValidateUser = function () {        

        $("#form-submit").validate({
            rules:
            {               
                MaPhong: {
                    required: true
                },
                HanDong: {
                    required: true
                },
                TrangThai: {
                    required: true
                }
            },
            messages:
            {
                MaPhong: {
                    required: "Vui lòng chọn phòng"
                },
                HanDong: {
                    required: "Vui lòng chọn hạn ngày đóng tiền"
                },
                TrangThai: {
                    required: "Vui lòng chọn trạng thái"
                }
            },
            submitHandler: function (form) {               
                self.GetValue();
                if (self.IsUpdate) {
                    self.UpdateUser(self.HoaDon);
                }
                else {
                    self.AddUser(self.HoaDon);
                }
            }
        });
    }

    self.GetValue = function () {

        self.HoaDon.MaPhongs = $("#MaPhong").val().map(Number);
        self.HoaDon.TrangThai = $("#TrangThai").val();
        self.HoaDon.HanDong = $.datepicker.formatDate("yy-mm-dd", $("#HanDong").datepicker("getDate"));
        self.HoaDon.GhiChu = $("#GhiChu").val();
    }

    Set.SetValue = function () {
        $("#MaPhong").val(self.HoaDon.MaPhong);
        $("#MaNha").val(self.HoaDon.MaNha);
        $("#MaNV").val(self.HoaDon.MaNV);
        $("#MaKH").val(self.HoaDon.MaKH);
        $("#TienCoc").val(self.HoaDon.TienCoc);
        $("#TrangThai").val(self.HoaDon.TrangThai);
        $("#GhiChu").val(self.HoaDon.GhiChu); 
        $("#NgayBatDau").val(self.HoaDon.NgayBatDau);
        if (self.HoaDon.NgayKetThucStr != "") {
            $("#NgayKetThuc").val(self.HoaDon.NgayKetThuc);
        }      
    }

    self.RenderHtmlByObject = function (view) {
        $("#MaPhong").val(view.MaPhong);
        $("#MaNha").val(view.MaNha);
        $("#MaNV").val(view.MaNV);
        $("#MaKH").val(view.MaKH);
        $("#TienCoc").val(view.TienCoc);
        $("#TrangThai").val(view.TrangThai);
        $("#GhiChu").val(view.GhiChu); 
        $("#NgayBatDau").datepicker("setDate", new Date(view.NgayBatDauStr));
        if (view.NgayKetThucStr != "") {
            $("#NgayKetThuc").datepicker("setDate", new Date(view.NgayKetThucStr));
        }
       

    }
    self.BindRoleHtml = function (data) {
        if (data !== null && data.length > 0) {
            var html = "<option value='0'>Chọn quyền</option>";
            $.each(data, function (key, item) {
                html += "<option value=" + item.Id + ">" + item.Name+"</option>";
            })
            $(".data-select2").html(html);
        }
    }
    self.ShowModalAddPhong = function (id) {
        self.HoaDon.MaNha = id;
        $("#titleModal").text("Thêm mới phòng");
        $(".btn-submit-format").text("Thêm mới");
        self.IsUpdate = false;
        $('#userModal').modal('show');
    }

    // thanh toans
    self.ValidateThanhToan = function () {

        $("#form-submit-bill").validate({
            rules:
            {
                TienDong: {
                    required: true
                },
                NguoiDong: {
                    required: true
                }
            },
            messages:
            {
                TienDong: {
                    required: "Vui lòng nhập tiền đóng"
                },
                NguoiDong: {
                    required: "Vui lòng nhập người đóng tiền"
                }
            },
            submitHandler: function (form) {
                var tiendong = $("#TienDong").val();
                var ghichu = $("#GhiChu").val();
                var nguoidong = $("#NguoiDong").val();
                var hoaDonBill = {
                    Id: parseInt($("#mahoadonhidden").val()),
                    NguoiDong: nguoidong,
                    GhiChu: ghichu,
                    TienDong: tiendong,
                    TrangThai: $("#TrangThaiBill").val(),
                    TongTien: parseInt($("#tongtienhidden").val())
                };

                self.PaymentBill(hoaDonBill);
                //self.GetValue();
                //if (self.IsUpdate) {
                //    self.UpdateUser(self.HoaDon);
                //}
                //else {
                //    self.AddUser(self.HoaDon);
                //}
            }
        });
    }

    self.PaymentBill = function (hoaDonPaymentBill) {
        $.ajax({
            url: '/Admin/HoaDon/PaymentBill',
            type: 'POST',
            dataType: 'json',
            data: {
                HoaDonModelView: hoaDonPaymentBill
            },
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                window.location.reload();
                //if (response.success) {
                //    tedu.notify('Cập nhật dữ liệu thành công', 'success');
                //    /*self.GetDataPaging(true);*/
                //    $('#userModal').modal('hide');
                //    window.location.reload();
                //}

            }
        })
    }


    $(document).ready(function () {
        self.GetDataPaging();
        self.ValidateUser();     
        self.ValidateThanhToan();
        /*self.ValidateAddFast();*/
        self.GetAllNha();
        self.GetAllPhong();
        self.GetAllKhachHang();
        self.GetAllNhanVien();

        //$(".formatdate").datepicker({
        //    dateFormat: 'mm/yy'
        //});
        $('#MaPhong').select2({
            placeholder: 'Chọn phòng'
        });
        /*$("#MaPhong").val(1);*/
        //$("#MaPhong").val(3);
        /* $("#MaPhong").select2('val', 3);*/

        //$('#MaPhong').val(3);
        //$('#MaPhong').select2().trigger('change');
        //$("#MaPhong").select2().val([0,3,4,5]).trigger("change");


        $('.formatdate').datepicker({
            changeMonth: true,
            changeYear: true,
            /*showDate:false,*/
            showButtonPanel: true,
            dateFormat: 'dd/mm/yy',
            //onClose: function (dateText, inst) {
            //    $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
            //}
        });

        $(".modal").on("hidden.bs.modal", function () {
            $(this).find('form').trigger('reset');
            $("form").validate().resetForm();
            $("label.error").hide();
            $(".error").removeClass("error");
        });

        $('#select-right').on('change', function () {
            $('input.form-search').val("");
            self.UserSearch.name = null;
            self.UserSearch.status = $(this).val();
            self.GetDataPaging(true);
        });
        $('#TrangThaiSearch').on('change', function () {
            $('input.form-search').val("");
            self.UserSearch.name = null;
            self.UserSearch.status = $(this).val();
            self.GetDataPaging(true);
        });

        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            self.GetDataPaging(true);
        });

        $('input.form-search').on('input', function (e) {
            self.UserSearch.name = $(this).val();
            self.GetDataPaging(true);
        });
       
    })
})(jQuery);