(function ($) {
    var self = this;
    self.Data = [];
    self.UserImages = {};
    self.IsUpdate = false;   
    self.HopDong = {
        Id:0,
        MaPhong:0,
        MaNha:0,
        MaNV:0,
        MaKH:0,
        NgayBatDau:"",
        NgayKetThuc:"",
        TienCoc:0,
        TrangThai:"",
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
                html += "<td>" + item.TenKhachHang + "</td>";
                html += "<td>" + (item.TenNhanVien != "" && item.TenNhanVien != null ? item.TenNhanVien:"") + "</td>";
                html += "<td>" + item.TenNha + "</td>";
                html += "<td>" + item.TenPhong + "</td>";
                html += "<td>" + item.NgayBatDauStr + "</td>";
                html += "<td>" + item.NgayKetThucStr + "</td>";
                html += "<td>" + item.TienCoc + "</td>";
                 
                var htmlControl = ""
                if (item.IsDeteled != true) {
                    htmlControl = "<button  class=\"btn btn-primary custom-button\" onClick=\"Update(" + item.Id + ")\"><i  class=\"bi bi-pencil-square\"></i></button>" +
                        "<button  class=\"btn btn-danger custom-button\" onClick=\"Deleted(" + item.Id + ")\"><i  class=\"bi bi-trash\"></i></button>";
                    html += "<td>" + item.TrangThaiStr + "</td>";  
                } else {
                    
                    html += "<td>Đã xóa</td>";  
                }
                html += "<td style=\"text-align: center;\">" + htmlControl + "</td>";                
                html += "</tr>";
            }
        }
        else {
            html += "<tr><td colspan=\"11\" style=\"text-align:center\">Không có dữ liệu</td></tr>";
        }
        $("#tblData").html(html);
    };
    self.Update = function (id) {
        if (id != null && id != "") {
            $(".txtPassword").hide();
            $("#titleModal").text("Cập nhật hợp đồng");
            $(".btn-submit-format").text("Cập nhật");
            /*$(".custom-format").attr("disabled", "disabled");*/
            self.GetById(id, self.RenderHtmlByObject);
            self.HopDong.Id = id;
            $('#userModal').modal('show');

            self.IsUpdate = true;
        }
    }

    self.GetById = function (id, renderCallBack) {
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/HopDong/GetById',
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
                        self.GetPhongByNhaUpdate(response.Data.MaNha, response.Data.MaPhong);
                        setTimeout(function () {
                            renderCallBack(response.Data);
                         /*   self.HopDong.Id = id;*/
                        }, 1000);
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

    //self.GetAllPhong = function () {
    //    $.ajax({
    //        url: '/Admin/Phong/GetAll',
    //        type: 'GET',
    //        dataType: 'json',
    //        beforeSend: function () {
    //        },
    //        complete: function () {
    //        },
    //        success: function (response) {
    //            var html = "<option value =\"\">Chọn phòng</option>";
    //            if (response.Data != null && response.Data.length > 0) {
    //                for (var i = 0; i < response.Data.length; i++) {
    //                    var item = response.Data[i];
    //                    html += "<option value =" + item.Id + ">" + item.TenPhong + "</option>";
    //                }
    //            }
    //            $("#MaPhong").html(html);
    //        }
    //    })
    //}

    //self.GetPhongByNhaUpdate = function () {
    //    $.ajax({
    //        url: '/Admin/Phong/GetPhongByMaNha',
    //        type: 'GET',
    //        dataType: 'json',
    //        data: {
    //            maNha: maNha
    //        },
    //        beforeSend: function () {
    //        },
    //        complete: function () {
    //        },
    //        success: function (response) {
    //            var html = "<option value =\"\">Chọn phòng</option>";
    //            if (response.Data != null && response.Data.length > 0) {
    //                for (var i = 0; i < response.Data.length; i++) {
    //                    var item = response.Data[i];
    //                    html += "<option value =" + item.Id + ">" + item.TenPhong + "</option>";
    //                }
    //            }
    //            $("#MaPhong").html(html);
    //        }
    //    })
    //}

    self.GetPhongByNha = function (tag) {
        var maNha = $(tag).val();
        $.ajax({
            url: '/Admin/Phong/GetPhongByMaNha',
            type: 'GET',
            dataType: 'json',
            data: {
                maNha: maNha
            },
            beforeSend: function () {
                /*Loading('show');*/
            },
            complete: function () {
                /*Loading('show');*/
            },
            success: function (response) {
                var html = "<option value =\"\">Chọn phòng</option>";
                if (response.Data != null && response.Data.length > 0) {
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.Id + ">" + item.TenPhong + "</option>";
                    }
                }
                $("#MaPhong").html(html);
            }
        })
    }
    self.GetPhongByNhaUpdate = function (maNha,maPhong) {
        $.ajax({
            url: '/Admin/Phong/GetPhongByMaNha',
            type: 'GET',
            dataType: 'json',
            data: {
                maNha: maNha
            },
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var html = "<option value =\"\">Chọn phòng</option>";
                if (response.Data != null && response.Data.length > 0) {
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        if (item.Id == maPhong) {
                            html += "<option value =" + item.Id + " selected>" + item.TenPhong + "</option>";
                        }
                        else {
                            html += "<option value =" + item.Id + ">" + item.TenPhong + "</option>";
                        }
                      
                    }
                }
                $("#MaPhong").html(html);
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
            tedu.confirm('Bạn có chắc muốn xóa hợp đồng này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/HopDong/Delete",
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
            url: '/Admin/HopDong/GetAllPaging',
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
            url: '/Admin/HopDong/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                HopDongModelView: userView
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
            url: '/Admin/HopDong/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                HopDongModelView: userView
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
                MaNha: {
                    required: true
                },
                MaPhong: {
                    required: true
                },
                MaKH: {
                    required: true
                },
                MaNV: {
                    required: true
                },
                NgayBatDau: {
                    required: true
                },
                TienCoc: {
                    required: true
                },   
                TrangThai: {
                    required: true
                }
            },
            messages:
            {
                MaNha: {
                    required: "Vui lòng chọn nhà"
                },
                MaPhong: {
                    required: "Vui lòng chọn phòng"
                },
                MaKH: {
                    required: "Vui lòng chọn khách hàng"
                },
                MaNV: {
                    maxlength: "Vui lòng chọn nhân viên"
                },
                NgayBatDau: {
                    required: "Vui lòng ngày bắt đầu"
                },
                TienCoc: {
                    required: "Vui lòng nhập tiền đặt cọc"
                },
                TrangThai: {
                    required: "Vui lòng chọn trạng thái"
                }
            },
            submitHandler: function (form) {               
                self.GetValue();
                if (self.IsUpdate) {
                    self.UpdateUser(self.HopDong);
                }
                else {
                    self.AddUser(self.HopDong);
                }
            }
        });
    }

    self.GetValue = function () {
        self.HopDong.MaPhong = $("#MaPhong").val();
        self.HopDong.MaNha = $("#MaNha").val();
        self.HopDong.MaNV = $("#MaNV").val();
        self.HopDong.MaKH = $("#MaKH").val();
        /*self.HopDong.NgayBatDau = $("#NgayBatDau").val();   */
        self.HopDong.NgayBatDau = $.datepicker.formatDate("yy-mm-dd", $("#NgayBatDau").datepicker("getDate"));
        self.HopDong.NgayKetThuc = $.datepicker.formatDate("yy-mm-dd", $("#NgayKetThuc").datepicker("getDate"));  
        self.HopDong.TienCoc = $("#TienCoc").val(); 
        self.HopDong.TrangThai = $("#TrangThai").val(); 
        self.HopDong.GhiChu = $("#GhiChu").val(); 
    }

    Set.SetValue = function () {
        $("#MaPhong").val(self.HopDong.MaPhong);
        $("#MaNha").val(self.HopDong.MaNha);
        $("#MaNV").val(self.HopDong.MaNV);
        $("#MaKH").val(self.HopDong.MaKH);
        $("#TienCoc").val(self.HopDong.TienCoc);
        $("#TrangThai").val(self.HopDong.TrangThai);
        $("#GhiChu").val(self.HopDong.GhiChu); 
        $("#NgayBatDau").val(self.HopDong.NgayBatDau);
        if (self.HopDong.NgayKetThucStr != "") {
            $("#NgayKetThuc").val(self.HopDong.NgayKetThuc);
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
        self.HopDong.MaNha = id;
        $("#titleModal").text("Thêm mới phòng");
        $(".btn-submit-format").text("Thêm mới");
        self.IsUpdate = false;
        $('#userModal').modal('show');
    }

    $(document).ready(function () {
        self.GetDataPaging();
        self.ValidateUser();     

        /*self.ValidateAddFast();*/
        self.GetAllNha();
        //self.GetAllPhong();
        self.GetAllKhachHang();
        self.GetAllNhanVien();
        $(".formatdate").datepicker({
            dateFormat: 'dd/mm/yy'
        });

        $(".modal").on("hidden.bs.modal", function () {
            $(this).find('form').trigger('reset');
            $("form").validate().resetForm();
            $("label.error").hide();
            self.IsUpdate = false;
            $("#titleModal").text("Thêm mới hợp đồng");
            $(".btn-submit-format").text("Thêm mới");
            self.HopDong.Id = 0;
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