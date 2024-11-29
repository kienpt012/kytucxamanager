(function ($) {
    var self = this;
    self.Data = [];
    self.UserImages = {};
    self.IsUpdate = false;   
    self.Phong = {
        Id: null,
        MaNha: "",
        PhongTu: "",
        DenPhong: "",
        TenPhong: "",
        DonGia: 0,
        SLNguoiMax: 0,
        ChieuDai: 0,
        ChieuRong: 0,
        MoTa: "",
        LoaiPhong: "",  
        TinhTrang: "",
        HinhAnh:""
    }
    self.UserSearch = {
        name: "",
        role: null,
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }
    self.lstRole = [];
    self.DicVu = [];
    self.ListDicVu = [];
    // dịch vụ
    self.DichVuPhong = [];
    self.MaPhongAddDichVu = 0;

    // thành viên
    self.ThanhVienPhong = [];
    self.ThanhVienPhongExist = [];
    self.MaPhongAddThanhVien = 0;

    self.addSerialNumber = function () {
        var index = 0;
        $("table tbody tr").each(function (index) {
            $(this).find('td:nth-child(1)').html(index + 1);
        });
    };
    self.Files = {};
    self.CategoryImage = {};

    self.RenderTableHtml = function (data) {
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                html += "<tr>";
                html += "<td>" + (++index) + "</td>";
                html += "<td>" + item.TenNha + "</td>";
                html += "<td>" + item.TenPhong + "</td>";
                html += "<td>" + item.DonGia + "</td>";
                html += "<td>" + item.SLNguoiMax + "</td>";
                html += "<td>" + item.ChieuDai + "</td>";
                html += "<td>" + item.ChieuRong + "</td>";                                                
                html += "<td>" + item.LoaiPhongStr + "</td>";
                html += "<td>" + item.TinhTrangStr + "</td>";      
                html += "<td style=\"text-align: center;\">" +                    
                    "<button  class=\"btn btn-primary custom-button\" onClick=\"Update(" + item.Id +")\"><i  class=\"bi bi-pencil-square\"></i></button>" +
                    "<button  class=\"btn btn-danger custom-button\" onClick=\"Deleted(" + item.Id +")\"><i  class=\"bi bi-trash\"></i></button>" +
                    "</td>";                
                html += "</tr>";
            }
        }
        else {
            html += "<tr><td colspan=\"10\" style=\"text-align:center\">Không có dữ liệu</td></tr>";
        }
        $("#tblData").html(html);
    };
    self.UploadFileImage = function () {
        var dataImage = new FormData();
        dataImage.append(0, self.CategoryImage);

        $.ajax({
            url: '/Admin/Phong/UploadImage',
            type: 'POST',
            contentType: false,
            processData: false,
            data: dataImage,
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
            }
        })
    }
    self.Update = function (id,manha) {
        if (id != null && id != "") {
            $(".txtPassword").hide();
            $("#titleModal").text("Cập nhật phòng");
            $(".btn-submit-format").text("Cập nhật");
            /*$(".custom-format").attr("disabled", "disabled");*/
            self.GetById(id, self.RenderHtmlByObject);
            self.Phong.Id = id;
            /*self.Phong.Id = id;*/
            $('#userModal').modal('show');

            self.IsUpdate = true;
        }
    }

    self.GetById = function (id, renderCallBack) {
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/Phong/GetById',
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
                $("#MaNhaFast").html(html);
            }
        })
    }

    self.GetPhongByNha = function (id, tag) {
        $(".custom-item").removeClass('active');
        $(tag).addClass('active');
        self.Phong.MaNha = id;

        $.ajax({
            url: '/Admin/Phong/GetPhongByNha',
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
                    var html = "<div class='tab-content-main'>";
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        var htmlDichVuKhach = "";
                        var srcimg = "/category-image/" + item.HinhAnh;
                            htmlDichVuKhach = "<p class='custom-p'>" +
                                "<button class='btn btn-primary' onclick='viewRoomMembers(" + item.Id+")'>Xem thành viên phòng</button>" +
                                "<button class='btn btn-success btn-custom-eidt' onClick='AddDichVu(" + item.Id + ") 'type='button'>Dịch Vụ</button>" +
                                "<button class='btn btn-primary btn-custom-remove' onClick='AddThanhVien(" + item.Id + ")' type='button'>Thêm Khách</button></p>";                          

                        html += "<div class='item-content'>" +
                            "<i class='fa-solid fa-person-shelter'></i ><span style='margin-left: 6px;'>" + item.TenPhong + "</span>" + htmlDichVuKhach +

                            "<div class='image-phong-tro' style='background-image:url(" + srcimg +")'></div>" +

                            "<p class='custom-p'>Chiều dài  : " + item.ChieuDai + "</p>" +

                            "<p class='custom-p'>Chiều rộng : " + item.ChieuRong + "</p>" +                           
                            "<p class='custom-p'>Đơn giá: " + item.DonGiaStr + "</p> " +
                        /*"<p class='custom-p'>SL người tối đa: " + item.SLNguoiMax + "</p>"+*/
                            "<p class='custom-p'>Số lượng người tối đa: " + item.SLNguoiMax + "</p>" +
                            "<p class='custom-p'> Tình trạng: " + item.TinhTrangStr + "</p> " +
                            "<button class='btn btn-info btn-custom-eidt' onClick=\"Update(" + item.Id + ")\" type='button'>Chỉnh sửa</button>" +
                            "<button class='btn btn-danger btn-custom-remove' onClick=\"Deleted(" + item.Id +")\" type='button'>Xóa</button></div>";
                    }
                    html += "</div>";
                    var htmlButton = '<button type="button" class="btn btn-success btn-right btn-addorupdate" style="float:right" onclick="ShowModalAddPhong(' + id + ')">' +
                        '<span style = "display: flex;  align-items: center;" ><i class="bi bi-plus-circle"></i> <span style="margin-left: 6px; padding-top: 2px;"> Tạo phòng</span></span></button>';
                    $('.tab-content').html(htmlButton + html);
                }
                
                $(".tab-content-main").html();
            }
        })
    }

    //self.UpdateStatus = function (id,status) {
    //    $.ajax({
    //        url: '/Admin/Phong/UpdateStatus',
    //        type: 'GET',
    //        dataType: 'json',
    //        data: {
    //            id: id,
    //            status: status
    //        },
    //        beforeSend: function () {
    //        },
    //        complete: function () {
    //        },
    //        success: function (response) {
    //            if (response.success) {
    //                self.GetDataPaging(true);
    //                tedu.notify('Cập nhật trạng thái thành công', 'success');
    //            }
    //        }
    //    })
    //}

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
            tedu.confirm('Bạn có chắc muốn xóa tài khoản này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Phong/Delete",
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
            url: '/Admin/Phong/GetAllPaging',
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

    self.GetUserById = function (id) {
        $.ajax({
            type: "GET",
            url: "/Admin/Phong/GetUserById",
            dataType: "json",
            data: {
                id: id
            },
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Status) {
                    var resUser = response.Data.UserVM;
                    self.KhachHang.Id = resUser.Id;
                    self.KhachHang.TenKH = resUser.FullName;
                    self.KhachHang.SoDienThoai = resUser.BirthDay;
                    self.KhachHang.Email = resUser.UserName;
                    self.KhachHang.CMND = resUser.Email;
                    self.KhachHang.GioiTinh = resUser.Address;
                    self.KhachHang.DiaChi = resUser.PhoneNumber;
                    //self.KhachHang.TenDangNhap = resUser.Avatar;
                    self.KhachHang.NgaySinh = resUser.Avatar;
                    Set.SetValue();
                }
            },
            error: function () {
            }
        });
    }

    self.AddUser = function (userView) {
        $.ajax({
            url: '/Admin/Phong/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                phongModelView: userView
            },
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    if (self.CategoryImage != null) {
                        self.UploadFileImage();
                    }
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

    self.AddPhongByNha = function (userView) {
        $.ajax({
            url: '/Admin/Phong/AddPhongByNha',
            type: 'POST',
            dataType: 'json',
            data: {
                phongModelView: userView
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
                    $('#AddFastModal').modal('hide');
                    window.location.reload();
                }
                //else {
                //    if (response.isPhoneExist) {
                //        $(".phone_number-exist").show().text("Phone đã tồn tại");
                //    }
                //    if (response.isMailExist) {
                //        $(".email-exist").show().text("Email đã tồn tại");
                //    }
                //}
            }
        })
    }


    self.UpdateUser = function (userView) {
        $.ajax({
            url: '/Admin/Phong/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                phongModelView: userView
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


    self.ValidateAddFast = function () {

        $("#form-submit-fast").validate({
            rules:
            {
                MaNhaFast: {
                    required: true
                },
                DonGiaFast: {
                    required: true
                },
                PhongTuFast: {
                    required: true
                },
                DenPhongFast: {
                    required: true
                },
                ChieuDaiFast: {
                    required: true
                },
                ChieuRongFast: {
                    required: true
                },
                LoaiPhongFast: {
                    required: true
                },
                MoTaFast: {
                    required: true
                },
                TinhTrangFast: {
                    required: true
                },
                SLNguoiMax: {
                    required: true
                }
            },
            messages:
            {
                MaNhaFast: {
                    required: "Tên phòng không được để trống"
                },
                DonGiaFast: {
                    required: "Đơn giá không được để trống"
                },
                PhongTuFast: {
                    required: "Phòng từ không được để trống"
                },
                DenPhongFast: {
                    required: "Phòng đến không được để trống"
                },
                ChieuDaiFast: {
                    required: "Chiều dài không được để trống"
                },
                ChieuRongFast: {
                    maxlength: "Chiều rộng không được quá 128 kí tự"
                },
                LoaiPhongFast: {
                    required: "Vui lòng chọn loại phòng"
                },
                MoTaFast: {
                    required: "Vui lòng nhập mô tả"
                },
                TinhTrangFast: {
                    required: "Vui lòng chọn nhập mô tả"
                },
                SLNguoiMax: {
                    required: "Vui lòng chọn nhập số lượng người"
                }
            },
            submitHandler: function (form) {
                self.Phong = {};
                self.Phong.MaNha = $("#MaNhaFast").val();
                self.Phong.PhongTu = $("#PhongTuFast").val();
                self.Phong.DenPhong = $("#DenPhongFast").val();
                self.Phong.DonGia = $("#DonGiaFast").val();
                self.Phong.SLNguoiMax = $("#SLNguoiMax").val();
                self.Phong.ChieuDai = $("#ChieuDaiFast").val();
                self.Phong.ChieuRong = $("#ChieuRongFast").val();
                self.Phong.MoTa = $("#MoTaFast").val();
                self.Phong.LoaiPhong = $("#LoaiPhongFast").val();
                self.Phong.TinhTrang = $("#TinhTrangFast").val(); 
                self.AddPhongByNha(self.Phong);
            }
        });
    }

    self.ValidateUser = function () {        

        $("#form-submit").validate({
            rules:
            {
                TenPhong: {
                    required: true
                },
                DonGia: {
                    required: true
                },
                ChieuDai: {
                    required: true
                },
                ChieuRong: {
                    required: true
                },
                LoaiPhong: {
                    required: true
                },
                MoTa: {
                    required: true
                },    
                SLNguoiMax: {
                    required: true
                }
            },
            messages:
            {
                TenPhong: {
                    required: "Tên phòng không được để trống"
                },
                DonGia: {
                    required: "Đơn giá không được để trống"
                },
                ChieuDai: {
                    required: "Chiều dài không được để trống"
                },
                ChieuRong: {
                    maxlength: "Chiều rộng không được quá 128 kí tự"
                },
                LoaiPhong: {
                    required: "Vui lòng chọn loại phòng"
                },
                MoTa: {
                    required: "Vui lòng nhập mô tả"
                },
                SLNguoiMax: {

                }
            },
            submitHandler: function (form) {               
                self.GetValue();
                if (self.IsUpdate) {
                    if (self.CategoryImage != null) {
                        self.UploadFileImage();
                    }
                    self.UpdateUser(self.Phong);
                }
                else {
                    self.AddUser(self.Phong);
                }
            }
        });
    }

    //self.RenderHtmlByObject = function (view) {
    //    $(".txtname").val(view.HinhAnh);
    //    self.Phong.HinhAnh = view.HinhAnh;
    //    if (view.HinhAnh != null && view.HinhAnh != "") {
    //        var html = "";
    //        html = "<div class=\"box-image custom-image\" style=\"background-image:url(/category-image/" + view.HinhAnh + ")\"><span onclick=\"removeImageViewServer(" + view.HinhAnh + ",this)\" class='remove-image'>X</span></div>";

    //        $(".productimages").append(html);

    //    }
    //}
    self.GetValue = function () {
        //self.Phong.MaNha = $("#MaNha").val();
        self.Phong.TenPhong = $("#TenPhong").val();
        self.Phong.DonGia = $("#DonGia").val();
        self.Phong.SLNguoiMax = $("#SLNguoiMax").val();
        self.Phong.ChieuDai = $("#ChieuDai").val();
        self.Phong.ChieuRong = $("#ChieuRong").val();   
        self.Phong.MoTa = $("#MoTa").val();   
        self.Phong.LoaiPhong = $("#LoaiPhong").val(); 
        self.Phong.TinhTrang = $("#TinhTrang").val(); 
        
    }

    Set.SetValue = function () {

        //$("#MaNha").val(self.Phong.MaNha);
        $("#TenPhong").val(self.Phong.TenPhong);
        $("#DonGia").val(self.Phong.DonGia);
        $("#SLNguoiMax").val(self.Phong.SLNguoiMax);
        $("#ChieuDai").val(self.Phong.ChieuDai);
        $("#ChieuRong").val(self.Phong.ChieuRong);
        $("#MoTa").val(self.Phong.MoTa);
        $("#LoaiPhong").val(self.Phong.LoaiPhong);
        $("#TinhTrang").val(self.Phong.TinhTrang); 
    }

    self.RenderHtmlByObject = function (view) {

        $("#MaNha").val(view.MaNha);
        $("#TenPhong").val(view.TenPhong);
        $("#DonGia").val(view.DonGia);
        $("#SLNguoiMax").val(view.SLNguoiMax);
        $("#ChieuDai").val(view.ChieuDai);
        $("#ChieuRong").val(view.ChieuRong);
        $("#MoTa").val(view.MoTa);
        $("#LoaiPhong").val(view.LoaiPhong);
        $("#TinhTrang").val(view.TinhTrang); 

        $(".txtname").val(view.HinhAnh);
        self.Phong.HinhAnh = view.HinhAnh;
        if (view.HinhAnh != null && view.HinhAnh != "") {
            var html = "";
            html = "<div class=\"box-image custom-image\" style=\"background-image:url(/category-image/" + view.HinhAnh + ")\"><span onclick=\"removeImageViewServer(this)\" class='remove-image'>X</span></div>";

            $(".productimages").append(html);

        }

    }
    self.removeImageViewServer = function (tag) {
        $(tag).parent().remove();
        self.Phong.HinhAnh = "";
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
        self.Phong.MaNha = id;
        $("#titleModal").text("Thêm mới phòng");
        $(".btn-submit-format").text("Thêm mới");
        self.IsUpdate = false;
        $('#userModal').modal('show');
    }

    // Dịch Vụ
    self.InitQuantity = function () {
        
        self.GetAllDichVu();
        $(".btn-create-row-quantity").click(function () {
            self.GetAllDichVuByPhong(self.MaPhongAddDichVu);           
        });
        $("#form-submit-quantity").on("submit", function (e) {
            e.preventDefault();

            $('#quantity table > tbody  > tr').each(function (index, tr) {
                // lấy id dịch vụ
               
                    var dichvuphong = {
                        MaDV: 0,
                        SoLuong: 0,
                        MaPhong: self.MaPhongAddDichVu
                    };
                var maDichVu = $(tr).find('.dichvuselect').val();
                if (maDichVu != 0) {
                    dichvuphong.MaDV = parseInt(maDichVu);
                }
                    //if (maDichVu != 0) {
                    //    dichvuphong.MaDV = parseInt(maDichVu);
                    //    var dichvu = self.DicVu.find(d => d.Id == maDichVu);
                    //    if (dichvu != null && dichvu.LoaiDV == 4) {
                    //        var soluong = $(tr).find('.soluong').val();
                    //        if (soluong != "") {
                    //            dichvuphong.SoLuong = parseInt(soluong);
                    //        }
                    //    }
                    //}
                var soluong = $(tr).find('.soluong').val();
                if (soluong != "") {
                    dichvuphong.SoLuong = parseInt(soluong);
                }   
                    self.DichVuPhong.push(dichvuphong);
                            
                // lấy số lượng nếu dịch vụ là trông xe
            });
            
            self.SaveDichVuPhong(self.DichVuPhong, self.MaPhongAddDichVu);
        })
    }
    self.SaveDichVuPhong = function (dichVuPhong,maPhong) {
        $.ajax({
            url: '/Admin/Phong/AddDichVuPhong',
            type: 'POST',
            dataType: 'json',
            data: {
                DichVuPhongModelViews: dichVuPhong,
                maPhong: maPhong
            },
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    tedu.notify('Cập nhật dịch vụ thành công', 'success');
                    $('#quantity').modal('hide');
                    window.location.reload();
                    //if (response.dichvutontai != null && response.dichvutontai.length > 0) {
                    //    $('#dichvutontai').modal('show');
                    //    var html = "";
                    //    for (var i = 0; i < response.dichvutontai.length; i++) {
                    //        var item = response.dichvutontai[i];
                    //        html += "<p>" + item + "</p>";
                    //    }
                    //    $('#dichvutontai .modal-body').html(html);
                    //} else {
                    //    window.location.reload();
                    //}

                } else {
                    if (response.dichvutontai != null && response.dichvutontai.length > 0) {
                        $('#quantity').modal('hide');
                        $('#dichvutontai').modal('show');
                        var html = "";
                        for (var i = 0; i < response.dichvutontai.length; i++) {
                            var item = response.dichvutontai[i];
                            html += "<p>" + item + "</p>";
                        }
                        $('#dichvutontai .modal-body').html(html);
                    }
                }

            }
        })
    }
    self.AddDichVu = function (id) {
        self.MaPhongAddDichVu = id;
        self.GetDichVuPhongForPhongId(id);
    }

    self.GetAllDichVuByPhong = function (maPhong) {
        $.ajax({
            url: '/Admin/Phong/GetDichvuPhongByMaPhong',
            type: 'GET',
            data: {
                maPhong: maPhong
            },
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Data != null && response.Data.length > 0) {
                    self.DicVu = response.Data;
                    var html = self.AddRowHtml();
                    $("#quantity #tblData").append(html);
                    $(".no-data").hide();
                }
                else {
                    alert("Đã hết dịch vụ để chọn");
                }
                
            }
        });
    };

    self.GetAllDichVu = function () {
        $.ajax({
            url: '/Admin/DichVu/GetAllDichVu',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                /*self.DicVu = response.Data;*/
                self.ListDicVu = response.Data;
            }
        });
    };

    self.GetDichVuByIdOrAll = function (id, isRenderAPI) {
        var html = "";
        var data = [];
        if (isRenderAPI) {
            data = self.ListDicVu;
            html = "<select class=\"form-select colors dichvuselect\" disabled onchange='GetDichVuSelect(this)'><option value=\"\"> Chọn dịch vụ </option>";
        }
        else {
            data = self.DicVu;
            html = "<select class=\"form-select colors dichvuselect\" onchange='GetDichVuSelect(this)' ><option value=\"\"> Chọn dịch vụ </option>";
        }
        if (data != null && data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                if (item.Id == id) {
                    html += "<option value=\"" + item.Id + "\" selected>" + (item.TenDV != "" ? item.TenDV : "") + "</option>";
                }
                else {
                    html += "<option value=\"" + item.Id + "\">" + (item.TenDV != "" ? item.TenDV : "") + "</option>";
                }
            }
        }
        html += "</select>";
        return html;
    }
    self.GetDichVuSelect = function (tag) {
        var dichVuSelected = $(tag).val();
        var tagParent = $(tag).parent().parent();
        if (dichVuSelected != "") {
            if (self.ListDicVu != null && self.ListDicVu.length > 0) {
                var dichvu = self.ListDicVu.find(d => d.Id == dichVuSelected);
                if (dichvu != null) {                                     
                    $(tagParent.find('.dongia')).text(dichvu.DonGiaStr);
                    if (dichvu.LoaiDV == 4) {
                        $(tagParent.find('.soluongtheodichvu')).append('<input type=\"number\" class=\"form-control soluong\" min=\"0\" required>');
                    } else {
                        $(tagParent.find('.soluongtheodichvu')).html('');
                    }
                }
            }
        } else {
            $(tagParent.find('.dongia')).text("");
        }
    }
    self.AddRowHtml = function () {
        var index = 0;
        var countTr = $(".count-tr tr").length;
        countTr = countTr + 1;
        var html = "<tr class=\"new\">";
        html += "<td>" + countTr + "</td>";
        html += "<td>" + self.GetDichVuByIdOrAll(0) + "</td>";
        html += "<td class ='dongia'></td>";
        html += "<td class='soluongtheodichvu'></td>";
        //html += "<td></td>";
        html += "<td style=\"text-align: center;\">" +
            /* "<button  class=\"btn btn-primary custom-button\" onClick=\"UpdateView()\"><i  class=\"bi bi-pencil-square custom-icon\"></i></button>" +*/
            "<button  class=\"btn btn-danger custom-button\" onClick=\"DeletedHtml(this)\"><i  class=\"bi bi-trash custom-icon\"></i></button>" +
            "</td>";
        html += "</tr>";
        return html;
    }
    self.DeletedHtml = function (tag) {
        $(tag).closest(".new").remove();
    }
    self.GetDichVuPhongForPhongId = function (id) {
        $.ajax({
            url: '/Admin/Phong/GetDichVuPhongByPhongId',
            type: 'GET',
            data: {
                id: id
            },
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                self.DichVuPhongRenderTableHtml(response.Data);
            }
        });
    };

    self.DichVuPhongRenderTableHtml = function (data) {
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                /*html += "<tr data-quantity=" + item.Id + " class=" + item.Id + ">";*/
                html += "<tr data-quantity=" + item.Id + " class=" + item.Id + ">";
                html += "<td>" + (++index) + "</td>";
                html += "<td>" + self.GetDichVuByIdOrAll(item.MaDV,true) + "</td>";
                html += "<td class ='dongia'>" + item.DichVu.DonGiaStr + "</td>";
                if (item.DichVu.LoaiDV == 4) {
                    html += "<td class='soluongtheodichvu'><input type=\"number\" class=\"form-control soluong\" min=\"0\" value='" + item.SoLuong + "' disabled required></td>";
                }
                else {
                    html += "<td class='soluongtheodichvu'></td>";
                }
                //html += "<td><input type=\"number\" class=\"form-control totalimport\" min=\"0\" required  disabled></td>";
                //html += "<td></td>";
                html += "<td style=\"text-align: center;\">" +
                    "<button type=\"button\" class=\"btn btn-primary custom-button\" onClick=\"UpdateQuantity(" + item.Id + ")\"><i  class=\"bi bi-pencil-square custom-icon\"></i></button>" +
                    "<button type=\"button\" class=\"btn btn-danger custom-button\" onClick=\"DeletedQuantity(" + item.Id + ")\"><i  class=\"bi bi-trash custom-icon\"></i></button>" +
                    "</td>";
                html += "</tr>";
            }
        }
        //else {
        //    html += "<tr class=\"no-data\"><td colspan=\"10\" style=\"text-align:center\">Không có dữ liệu</td></tr>";
        //}
        $("#quantity #tblData").html(html);
        $('#quantity').modal('show');
    };
    self.UpdateQuantity = function (id) {
        if (id > 0) {
            /*self.ListUpdateQuantity.push(id);*/
            var classNameSelect = "." + id.toString() + " select";

            var classNameInput = "." + id.toString() + " .totalimport" + " , " + "." + id.toString() + " .soluong";
            var className = classNameSelect + "," + classNameInput;
            $(className).removeAttr('disabled');
        }
    }
    self.DeletedQuantity = function (id) {
        if (id > 0) {           
            var className = "." + id.toString();
            $(className).remove();
        }
    }
    // thêm thành viên start
    self.InitThanhVien = function () {
        self.GetAllKhachHang();
        $(".btn-create-row-thanhvien").click(function () {
            var html = self.AddRowThanhVienHtml();
            $("#thanhvien #tblData").append(html);
            $(".no-data").hide();
        });
        $("#form-submit-thanhvien").on("submit", function (e) {
            e.preventDefault();

            $('#thanhvien table > tbody  > tr').each(function (index, tr) {
                // lấy id dịch vụ
                var thanhvienphong = {
                    MaKH: 0,
                    MaPhong: self.MaPhongAddThanhVien
                };
                var maThanhVien = $(tr).find('.dichvuselect').val();
                if (maThanhVien != 0) {
                    thanhvienphong.MaKH = parseInt(maThanhVien);
                }

                self.ThanhVienPhong.push(thanhvienphong);
            });

            self.SaveThanhVienPhong(self.ThanhVienPhong, self.MaPhongAddThanhVien);
        })
    }
    self.SaveThanhVienPhong = function (dichVuPhong, maPhong) {
        $.ajax({
            url: '/Admin/Phong/AddThanhVienPhong',
            type: 'POST',
            dataType: 'json',
            data: {
                ThanhVienPhongModelViews: dichVuPhong,
                maPhong: maPhong
            },
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.success) {
                    tedu.notify('Cập nhật dịch vụ thành công', 'success');
                    $('#quantity').modal('hide');
                    window.location.reload();
                }
            }
        })
    }
    self.AddThanhVien = function (id) {
        self.MaPhongAddThanhVien = id;
        self.GetThanhVienPhongForPhongId(id);
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
                self.KhachHang = response.Data;
            }
        });
    };

    self.GetThanhVienByIdOrAll = function (id, isRenderAPI) {
        var html = "";
        if (isRenderAPI) {
            html = "<select class=\"form-select colors dichvuselect\" disabled onchange='GetKhachHangSelect(this)'><option value=\"\"> Chọn Thành Viên </option>";
        }
        else {
            html = "<select class=\"form-select colors dichvuselect\" onchange='GetKhachHangSelect(this)' ><option value=\"\"> Chọn Thành Viên </option>";
        }
        if (self.KhachHang != null && self.KhachHang.length > 0) {
            for (var i = 0; i < self.KhachHang.length; i++) {
                var item = self.KhachHang[i];
                if (item.Id == id) {
                    html += "<option value=\"" + item.Id + "\" selected>" + (item.TenKH != "" ? item.TenKH : "") + "</option>";
                }
                else if (self.ThanhVienPhongExist.indexOf(item.Id ) == -1) {
                    html += "<option value=\"" + item.Id + "\">" + (item.TenKH != "" ? item.TenKH : "") + "</option>";
                }
            }
        }
        html += "</select>";
        return html;
    }
    self.GetKhachHangSelect = function (tag) {
        var dichVuSelected = $(tag).val();
        var tagParent = $(tag).parent().parent();
        if (dichVuSelected != "") {
            if (self.KhachHang != null && self.KhachHang.length > 0) {
                var dichvu = self.KhachHang.find(d => d.Id == dichVuSelected);
                if (dichvu != null) {
                    $(tagParent.find('.sdt')).text(dichvu.SoDienThoai);
                    $(tagParent.find('.gioitinh')).text(dichvu.GioiTinhStr);
                    $(tagParent.find('.diachi')).text(dichvu.Email);
                    //if (dichvu.LoaiDV == 4) {
                    //    $(tagParent.find('.soluongtheodichvu')).append('<input type=\"number\" class=\"form-control soluong\" min=\"0\" required>');
                    //} else {
                    //    $(tagParent.find('.soluongtheodichvu')).html('');
                    //}
                }
            }
        } else {
            $(tagParent.find('.dongia')).text("");
        }
    }
    self.AddRowThanhVienHtml = function () {
        var index = 0;
        var countTr = $(".count-tr-thanh-vien tr").length;
        countTr = countTr + 1;
        var html = "<tr class=\"new\">";
        html += "<td>" + countTr + "</td>";
        html += "<td>" + self.GetThanhVienByIdOrAll(0) + "</td>";
        html += "<td class ='sdt'></td>";
        html += "<td class='gioitinh'></td>";
        html += "<td class='diachi'></td>";
        //html += "<td></td>";
        html += "<td style=\"text-align: center;\">" +
            "<button  class=\"btn btn-danger custom-button\" onClick=\"DeletedHtml(this)\"><i  class=\"bi bi-trash custom-icon\"></i></button>" +
            "</td>";
        html += "</tr>";
        return html;
    }
    self.DeletedThanhVienHtml = function (tag) {
        $(tag).closest(".new").remove();
    }
    self.GetThanhVienPhongForPhongId = function (id) {
        $.ajax({
            url: '/Admin/Phong/GetThanhVienPhongByPhongId',
            type: 'GET',
            data: {
                id: id
            },
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                self.ThanhVienPhongRenderTableHtml(response.Data);
                if (response.Data != null && response.Data.length > 0) {
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        self.ThanhVienPhongExist.push(item.MaKH);
                    }
                }
                //console.log(self.ThanhVienPhongExist);
            }
        });
    };

    self.ThanhVienPhongRenderTableHtml = function (data) {
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var htmlEdit = "";
                if (true) {
                    htmlEdit = "<button type=\"button\" class=\"btn btn-primary custom-button\" onClick=\"UpdateThanhVien(" + item.Id + ")\"><i  class=\"bi bi-pencil-square custom-icon\"></i></button>" +
                        "<button type=\"button\" class=\"btn btn-danger custom-button\" onClick=\"DeletedThanhVien(" + item.Id + ")\"><i  class=\"bi bi-trash custom-icon\"></i></button>";
                }
                html += "<tr data-quantity=" + item.Id + " class=" + item.Id + ">";
                html += "<td>" + (++index) + "</td>";
                html += "<td>" + self.GetThanhVienByIdOrAll(item.MaKH, true) + "</td>";
                html += "<td class ='sdt'>" + item.KhachHang.SoDienThoai + "</td>";
                html += "<td class ='gioitinh'>" + item.KhachHang.GioiTinhStr + "</td>";
                html += "<td class ='diachi'>" + item.KhachHang.Email + "</td>";
                html += "<td style=\"text-align: center;\">" + htmlEdit + "</td>";
                html += "</tr>";
            }
        }
        //else {
        //    html += "<tr class=\"no-data\"><td colspan=\"10\" style=\"text-align:center\">Không có dữ liệu</td></tr>";
        //}
        $("#thanhvien #tblData").html(html);
        $('#thanhvien').modal('show');
    };
    self.UpdateThanhVien = function (id) {
        if (id > 0) {
            /*self.ListUpdateQuantity.push(id);*/
            var classNameSelect = "." + id.toString() + " select";

            var classNameInput = "." + id.toString() + " .totalimport" + " , " + "." + id.toString() + " .soluong";
            var className = classNameSelect + "," + classNameInput;
            $(className).removeAttr('disabled');
        }
    }
    self.DeletedThanhVien = function (id) {
        if (id > 0) {
            var className = "." + id.toString();
            $(className).remove();
        }
    }
    // thêm thành viên end







    $(document).ready(function () {
        self.GetDataPaging();
        self.ValidateUser();     
        self.ValidateAddFast();
        self.GetAllNha();

        self.InitQuantity();
        self.InitThanhVien();

        $(".modal").on("hidden.bs.modal", function () {
            $(this).find('form').trigger('reset');
            $("form").validate().resetForm();
            $("label.error").hide();
            $(".error").removeClass("error");
            self.MaPhongAddDichVu = 0;
            self.MaPhongAddThanhVien = 0;
            self.DichVuPhong = [];
            self.ThanhVienPhong = [];
            $(".productimages .custom-image").remove();
        });

        $('#select-right').on('change', function () {
            $('input.form-search').val("");
            self.UserSearch.name = null;
            self.UserSearch.role = $(this).val();
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

        $('#productimages').on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            if (files != null && files.length > 0) {
                var fileExtension = ['jpeg', 'jpg', 'png'];

                for (var i = 0; i < files.length; i++) {
                    var html = "";
                    if ($.inArray(files[i].type.split('/')[1].toLowerCase(), fileExtension) == -1) {
                        alert("Only formats are allowed : " + fileExtension.join(', '));
                    }
                    else {
                        files[i].name = files[i].name.replace(/ /g, "").toLowerCase();
                        self.Phong.HinhAnh = files[i].name;
                        self.CategoryImage = files[i];
                        var src = URL.createObjectURL(files[i]);

                        html = "<div class=\"box-image custom-image\" style=\"background-image:url(" + src + ")\"></div>";

                        $(".productimages").html(html);
                    }
                }
            }

        });
       
    })
})(jQuery);