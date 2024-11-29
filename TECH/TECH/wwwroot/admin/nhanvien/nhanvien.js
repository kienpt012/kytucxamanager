(function ($) {
    var self = this;
    self.Data = [];
    self.UserImages = {};
    self.IsUpdate = false;    
    //self.User = {
    //    id: null,
    //    full_name: null,
    //    password: "",
    //    email: "",
    //    address: "",
    //    phone_number: "",
    //    role: 0
    //}
    self.NhanVien = {
        Id: null,
        TenNV:"",
        SoDienThoai:"",
        Email:"",
        CMND: "",
        GioiTinh: "",
        DiaChi: "",
        TenDangNhap: "",
        MatKhau: "",
        NgaySinh: null,
        Role:0
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
    self.Files = {};

    self.RenderTableHtml = function (data) {
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                html += "<tr>";
                html += "<td>" + (++index) + "</td>";
                html += "<td>" + item.TenNV + "</td>";
                html += "<td>" + item.Email + "</td>";
                html += "<td>" + item.SoDienThoai + "</td>";
                /*html += "<td>" + item.SoDienThoai + "</td>";*/
                html += "<td>" + item.TenDangNhap + "</td>";
                html += "<td>" + item.NgaySinhStr + "</td>";
                html += "<td>" + item.GioiTinhStr + "</td>";
                html += "<td>" + item.CMND + "</td>";                                                
                html += "<td>" + item.DiaChi + "</td>";
             /*   html += "<td>" + (item.role == 1 ? "Khách hàng " :"Quản trị viên") + "</td>";      */         
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
    self.Update = function (id) {
        if (id != null && id != "") {
            $(".txtPassword").hide();
            $("#titleModal").text("Cập nhật tài khoản");
            $(".btn-submit-format").text("Cập nhật");
            /*$(".custom-format").attr("disabled", "disabled");*/
            self.GetById(id, self.RenderHtmlByObject);
            self.NhanVien.Id = id;
            $('#userModal').modal('show');

            self.IsUpdate = true;
        }
    }

    self.GetById = function (id, renderCallBack) {
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/NhanVien/GetById',
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

    self.UpdateStatus = function (id,status) {
        $.ajax({
            url: '/Admin/NhanVien/UpdateStatus',
            type: 'GET',
            dataType: 'json',
            data: {
                id: id,
                status: status
            },
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.success) {
                    self.GetDataPaging(true);
                    tedu.notify('Cập nhật trạng thái thành công', 'success');
                }
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
            tedu.confirm('Bạn có chắc muốn xóa tài khoản này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/NhanVien/Delete",
                    data: { id: id },
                    beforeSend: function () {
                    },
                    success: function () {
                        tedu.notify('Đã xóa thành công', 'success');
                        self.GetDataPaging(true);
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
            url: '/Admin/NhanVien/GetAllPaging',
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
            self.NhanVien.Id = 0;
            $('#CreateOrUpdate').modal('show')
        })

        // hủy add và edit
        $(".cs-close-addedit,.btn-cancel-addedit").click(function () {
            $("#CreateEdit").css("display", "none");
        })

        

        $(".filesImages").change(function () {
            var files = $(this).prop('files')[0];

            var t = files.type.split('/').pop().toLowerCase();

            if (t != "jpeg" && t != "jpg" && t != "png" && t != "bmp" && t != "gif") {
                alert('Vui lòng chọn một tập tin hình ảnh hợp lệ!');
                return false;
            }

            if (files.size > 2048000) {
                alert('Kích thước tải lên tối đa chỉ 2Mb');
                //$("#avatar").val("");
                return false;
            }

            var img = new Image();
            img.src = URL.createObjectURL(files);
            img.onload = function () {
                CheckWidthHeight(this.width, this.height);
            }
            var CheckWidthHeight = function (w, h) {
                if (w <= 300 && h <= 300) {
                    alert("Ảnh tối thiểu 300 x 300 px");
                }
                else {
                    $(".box-avatar").css({ 'background': 'url(' + img.src + ')', 'display': 'block' });                   
                    self.UserImages = files;
                    //console.log(self.UserImages);
                }
            }

        })

        $(".btn-submit-search").click(function () {
            var id = $("#code_user_search").val();
            var fullName = $('#fullname_user_search').val();
            var userName = $('#name_user_search').val();
            var email = $('#email_user_search').val();
            var address = $('#address_user_search').val();
            var phoneNumber = $('#phone_user_search').val();
            var birthDay = $('#birthday_user_search').val();

            self.UserSearch = {
                Id: id,
                FullName: fullName,
                UserName: userName,
                Email: email,
                Address: address,
                Phone: phoneNumber,
                Birthday: birthDay,
            }
            self.GetUser(self.UserSearch);
        })

        $('body').on('click', '.btn-edit', function () {
            $(".user .modal-title").text("Chỉnh sửa thông tin người dùng");
            var id = $(this).attr('data-id');
            if (id !== null && id !== undefined) {
                self.GetUserById(id);
                $('#create').modal('show');
            }
        })

        $('.add-role').click(function () {
            $('#AddRole').modal('show');
        })

        $('body').on('click', '.btn-delete', function () {
            var id = $(this).attr('data-id');
            var fullname = $(this).attr('data-fullname');
            if (id !== null && id !== '') {
                self.confirmUser(fullname, id);
            }
        })
        $(".add-image").click(function () {
            $("#file-input").click();
        })

        $('body').on('click', '.btn-role-user', function () {
            var id = $(this).attr('data-id');
            $("#user_id").val(id);
            //self.GetAllRoles(id);           
        })

        $('body').on('click', '.btn-set-role', function () {
            var userId = parseInt($("#user_id").val());
            $.each($("#lst-role tr"), function (key, item) {
                var check = $(item).find('.ckRole').prop('checked');
                if (check == true) {
                    var id = parseInt($(item).find('.ckRole').val());
                    self.lstRole.push({
                        UserId: userId,
                        RoleId: id
                    });
                }
            })
            if (self.lstRole.length > 0) {
                self.SaveRoleForUser(self.lstRole, userId);
            }

        })

        $('.filesImages').on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            if (files != null && files.length > 0) {
                var fileExtension = ['jpeg', 'jpg', 'png'];
                var html = "";
                for (var i = 0; i < files.length; i++) {
                    if ($.inArray(files[i].type.split('/')[1].toLowerCase(), fileExtension) == -1) {
                        alert("Only formats are allowed : " + fileExtension.join(', '));
                    }
                    else {
                        var src = URL.createObjectURL(files[i]);
                        html += "<div class=\"box-item-image\"> <div class=\"image-upload item-image\" style=\"background-image:url(" + src + ")\"></div></div>";
                    }
                }
                if (html != "") {
                    $(".image-default").hide();
                    $(".box-images").html(html);
                }
            }

        });
    }

    self.confirmUser = function (nameUser, id) {
        bootbox.confirm({
            message: '<div class="title-delete"><p>Bạn có chắc muốn xóa người dùng này?</p><p>' + nameUser + '</p></div>',
            centerVertical: true,
            buttons: {
                confirm: {
                    label: 'Đồng ý',
                    className: 'btn-success pull-left'
                },
                cancel: {
                    label: 'Hủy',
                    className: 'btn-danger '
                }
            },
            callback: function (result) {
                if (result === true) {
                    $.ajax({
                        type: "POST",
                        url: "/Admin/NhanVien/Delete",
                        dataType: "json",
                        data: {
                            id: id
                        },
                        beforeSend: function () {
                        },
                        complete: function () {
                        },
                        success: function (res) {
                            if (res.status) {
                                self.GetUser();
                                Notifly(res.message, "success");
                            }
                            else {
                                $.notify(res.message, 'error');
                            }
                        },
                        error: function () {
                        }
                    });
                }
            }
        });
    }

    self.SaveRoleForUser = function (userRoles, userId) {
        $.ajax({
            type: "POST",
            url: "/Admin/NhanVien/SaveRoleForUser",
            data: {
                userRoles: userRoles,
                userId: userId
            },
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Status == true) {
                    $('#assignrole').modal('hide');
                    $.notify("Gán quyền thành công", 'success');
                }
                else {
                    $.notify("Gán quyền không thành công", 'error');
                }
                self.lstRole = [];
            },
            error: function (eror) {
                console.log(eror);
            }
        });
    }


    self.AddImageAvatar = function () {
        var dataImage = new FormData();
        dataImage.append("image1", self.UserImages);
        $.ajax({
            url: '/Admin/NhanVien/UploadImageAvatar',
            type: 'POST',
            contentType: false,
            processData: false,
            data: dataImage,
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
            }
        })
    }

    self.GetRoleByUserId = function (userId) {
        $.ajax({
            type: "GET",
            url: "/Admin/NhanVien/GetRoleByUserId",
            data: {
                userId: userId
            },
            dataType: "json",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Status) {
                    var data = response.Data;
                    $.each($("#lst-role tr"), function (key, item) {
                        $.each(data, function (i, items) {
                            if (items.RoleId === parseInt($(item).data("id"))) {
                                $(item).find(".ckRole").prop("checked", true);
                            }
                        })

                    })
                }
                $('#assignrole').modal('show');

            },
            error: function () {
            }
        });
    }

    self.GetUserById = function (id) {
        $.ajax({
            type: "GET",
            url: "/Admin/NhanVien/GetUserById",
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
                    self.NhanVien.Id = resUser.Id;
                    self.NhanVien.TenNV = resUser.FullName;
                    self.NhanVien.SoDienThoai = resUser.BirthDay;
                    self.NhanVien.Email = resUser.UserName;
                    self.NhanVien.CMND = resUser.Email;
                    self.NhanVien.GioiTinh = resUser.Address;
                    self.NhanVien.DiaChi = resUser.PhoneNumber;
                    self.NhanVien.TenDangNhap = resUser.Avatar;
                    self.NhanVien.NgaySinh = resUser.Avatar;
                    Set.SetValue();
                }
            },
            error: function () {
            }
        });
    }
    // Set value default
    self.SetValueDefault = function () {
        self.User.Id = null;
        $("#fullname").val("").attr("placeholder", "Nhập tên người dùng");
        $("#mobile").val("").attr("placeholder", "Nhập số điện thoại");
        $("#birthday").val("").attr("placeholder", "Ngày sinh");
        $("#email").val("").attr("placeholder", "Email");
        $("#username").val("").attr("placeholder", "Tên đăng nhập");
        $("#password").val("").attr("placeholder", "Mật khẩu");
        $("#address").val("").attr("placeholder", "Địa chỉ");
        $("#confirm_password").val("").attr("placeholder", "Nhập lại mật khẩu");
        $(".box-avatar").css("display", "none");
    }
    // Get User
    self.GetUser = function () {
        $.ajax({
            type: "GET",
            url: "/Admin/NhanVien/GetAllUser",
            dataType: "json",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var data = response
                if (data != null && data.length > 0) {
                    var html = "";
                    $.each(data, function (key, item) {

                        html += "<tr>" +
                            "<td></td>" +
                            "<td>" + item.FullName + "</td>" +
                            '<td> <div class="img-avatar" style="background:url(/image/admin/avatar.jpg)"><img src="/' + (item.Avatar != null ? item.Avatar : "image/admin/avatar.jpg") + '"/></div></td>' +
                            "<td>" + item.UserName + "</td>" +
                            "<td>" + item.PhoneNumber + "</td>" +
                            "<td>" + (item.Email !== null && item.Email !== "" ? item.Email : "") + "</td>" +
                            "<td>" + (item.BirthDay !== null ? dateFormatJson(item.BirthDay) : "") + "</td>" +
                            "<td>" + (item.Address !== null && item.Address !== "" ? item.Address : "") + "</td>" +
                            '<td>' +
                            '<a class="btn-role-user fa fa-user-secret fa-lg" title = "gán role" data-id=' + item.Id + ' data-id=' + item.Id + '  href="javascript:void(0)" ></a >' +
                            '<a class="btn-edit fa fa-pencil-square fa-lg" title = "Sửa" data-id=' + item.Id + ' data-id=' + item.Id + '  href="javascript:void(0)" ></a >' +
                            '<a class="btn-delete fa fa-trash-o fa-lg" data-id=' + item.Id + ' data-fullname ="' + item.FullName + '" title="xóa" href="javascript:void(0)"></a>' +
                            '</td >' +
                            "</tr>";
                    });
                }
                else {
                    html += '<tr><td colspan="8"> <a class="come-black" href="javascript:void(0)">Không có dữ liệu</a>  </td></tr>'
                }

                $(".table-content").html(html);

                self.addSerialNumber();
            },
            error: function () {
            }
        });
    }

    self.AddUser = function (userView) {
        $.ajax({
            url: '/Admin/NhanVien/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                NhanVienModelView: userView
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
                    self.GetDataPaging(true);
                    $('#userModal').modal('hide');
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

    self.IsMailExist = function (mail) {
        $.ajax({
            url: '/Admin/NhanVien/IsEmailExist',
            type: 'POST',
            dataType: 'json',
            data: {
                email: mail
            },
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    Notifly("Thêm mới thành công", "success");
                }
            }
        })
    }


    self.UpdateUser = function (userView) {
        $.ajax({
            url: '/Admin/NhanVien/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                NhanVienModelView: userView
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
                    self.GetDataPaging(true);
                    $('#userModal').modal('hide');
                }
               
            }
        })
    }

    self.ValidateUser = function () {        
        jQuery.validator.addMethod("headphone", function (value, element) {
            var vnf_regex = /((032|033|034|035|036|037|038|039|056|058|059|070|076|077|078|079|081|082|083|084|085|086|087|088|089|090|091|092|093|094|096|097|098|099)+([0-9]{7})\b)/g;
            return vnf_regex.test(value);
        });
        jQuery.validator.addMethod("rigidemail", function (value, element) {
            var testemail = /^([^@\s]+)@((?:[-a-z0-9]+\.)+[a-z]{2,})$/i;
            return testemail.test(value);
        });
        $("#form-submit").validate({
            rules:
            {
                TenNV: {
                    required: true,
                    minlength: 8,
                },
                SoDienThoai: {
                    required: true,
                    headphone: true,
                    minlength: 10,
                    min: 0,
                    maxlength: 10,
                    number: isNaN
                },
                MatKhau: {
                    required: true,
                    minlength: 8,
                },
                confirm_password: {
                    required: true,
                    equalTo: "#MatKhau"
                },
                Email: {
                    required: true,
                    rigidemail: true
                },
                Role: {
                    required: true
                },
                DiaChi: {
                    maxlength:129
                },
                GioiTinh: {
                    required: true,
                },
                CMND: {
                    required: true,
                },
                TenDangNhap: {
                    required: true,
                }
            },
            messages:
            {
                TenNV: {
                    required: "Họ tên không được để trống",
                    minlength: "Tên tối thiểu 8 kí tự",
                },
                SoDienThoai: {
                    required: "Số điện thoại không được để trống",
                    headphone: "Số điện thoại không hợp lệ"
                },
                MatKhau: {
                    required: "Mật khẩu không được để trống",
                    minlength: "Mật khẩu tối thiểu 8 kí tự",
                },
                confirm_password: {
                    required:"Vui lòng nhập lại mật khẩu",
                    equalTo: "Mật khẩu không đúng.",
                },
                Email: {
                    required: "Email không được để trống",
                    rigidemail: "Email không đúng định dạng",
                },
                Role: {
                    required: "Loại tài khoản không được để trống"
                },
                DiaChi: {
                    maxlength: "Địa chỉ nhập không được quá 128 kí tự"
                },
                GioiTinh: {
                    required: "Vui lòng nhập giới tính"
                },
                CMND: {
                    required: "Vui lòng nhập CMND"
                },
                TenDangNhap: {
                    required: "Vui lòng nhập tên đăng nhập"
                }
            },
            submitHandler: function (form) {   
                var checkAlloweChars = /[\d`!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/.test($("#full_name").val());
                if (checkAlloweChars == true) {
                    $("#full_name-error").show().text("Họ tên không chứa số và kí tự đặc biệt");
                    return;
                }
                self.GetValue();
                if (self.IsUpdate) {
                    self.UpdateUser(self.NhanVien);
                }
                else {
                    self.AddUser(self.NhanVien);
                }
            }
        });
    }

    self.GetValue = function () {
        self.NhanVien.TenNV = $("#TenNV").val();
        self.NhanVien.SoDienThoai = $("#SoDienThoai").val();
        self.NhanVien.Email = $("#Email").val();
        self.NhanVien.CMND = $("#CMND").val();
        self.NhanVien.GioiTinh = $("#GioiTinh").val();
        self.NhanVien.DiaChi = $("#DiaChi").val();     
        self.NhanVien.TenDangNhap = $("#TenDangNhap").val();     
        self.NhanVien.MatKhau = $("#MatKhau").val();     
        self.NhanVien.Role = $("#Role").val();
        self.NhanVien.NgaySinh = $.datepicker.formatDate("yy-mm-dd", $("#NgaySinh").datepicker("getDate"));
    }

    Set.SetValue = function () {
        if (self.User.Id !== null && self.User.Id !== '') {
            $("#TenNV").val(self.NhanVien.TenNV);
            $("#SoDienThoai").val(self.NhanVien.SoDienThoai);
            $("#Email").val(self.NhanVien.Email);
            $("#CMND").val(self.NhanVien.CMND);
            $("#GioiTinh").val(self.NhanVien.GioiTinh);
            $("#DiaChi").val(self.NhanVien.DiaChi);
            $("#TenDangNhap").val(self.NhanVien.TenDangNhap);
            $("#MatKhau").val(self.NhanVien.MatKhau);
            $("#NgaySinh").val(self.NhanVien.NgaySinh);
            $("#Role").val(self.NhanVien.Role);



            //var dataSelecteRole = $('.data-select2').select2("data");
            //if (dataSelecteRole != null && dataSelecteRole.length > 0) {
            //    for (var i = 0; i < dataSelecteRole.length; i++) {
            //        var item = dataSelecteRole[i];
            //        if (item.id != null) {
            //            self.User.Roles.push(parseInt(item.id));
            //        }
            //    }
            //}

            //$("#NgaySinh").val(moment(self.NhanVien.NgaySinh).format('DD/MM/YYYY'));

          /*  $(".box-avatar").css({ 'background': 'url(/' + (self.User.Avatar !== null ? self.User.Avatar : "image/admin/avatar.jpg") + ')', 'display': 'block' });*/
        }
    }

    self.RenderHtmlByObject = function (view) {
        $("#TenNV").val(view.TenNV);
        $("#SoDienThoai").val(view.SoDienThoai);
        $("#Email").val(view.Email);
        $("#CMND").val(view.CMND);
        $("#GioiTinh").val(view.GioiTinh);
        $("#DiaChi").val(view.DiaChi);
        $("#TenDangNhap").val(view.TenDangNhap);
        $("#MatKhau").val(view.MatKhau);
        $("#confirm_password").val(view.MatKhau);              
        //$("#NgaySinh").val(view.NgaySinh);
      /*  $('#NgaySinh').datepicker("setDate", view.NgaySinh);*/

        $("#NgaySinh").datepicker("setDate", new Date(view.NgaySinhStr));

        //var parsedDate = $.datepicker.parseDate('yy-mm-dd', view.NgaySinh);

        //$('#NgaySinh').datepicker('setDate', parsedDate);


        $("#Role").val(view.Role);
    }

    self.SubmitUser = function (user) {
        var form_data = new FormData();

        form_data.append("Id", self.User.Id);
        form_data.append("FullName", self.User.FullName);
        form_data.append("PhoneNumber", self.User.PhoneNumber);
        form_data.append("Email", self.User.Email);
        form_data.append("Address", self.User.Address);
        form_data.append("UserName", self.User.UserName);
        form_data.append("Password", self.User.Password);

        $.ajax({
            type: "POST",
            url: "/Admin/NhanVien/SaveEntity",
            data: form_data,
            contentType: false,
            processData: false,
            beforeSend: function () {
            },
            complete: function () {
                $(".add-edit-user").css("display", "inline-block");
            },
            success: function (response) {
                if (response.Status == true) {
                    $('#create').modal('hide');
                    $.notify(response.message, 'success');
                }
                else {
                    $.notify(response.message, 'error');
                }
            },
            error: function (eror) {
                console.log(eror);
            }
        });
    }

    self.GetAllRole = function () {
        $.ajax({
            type: "GET",
            url: "/Admin/NhanVien/GetAll",
            dataType: "json",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Data !== null && response.Data.length > 0) {
                    self.BindRoleHtml(response.Data)
                }
            },
            error: function () {
            }
        });
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

    $(document).ready(function () {
        self.GetDataPaging();
        self.ValidateUser();      

        $("#NgaySinh").datepicker({
            dateFormat: 'dd/mm/yy'
        });

        $(".modal").on("hidden.bs.modal", function () {
            $(this).find('form').trigger('reset');
            $("form").validate().resetForm();
            $("label.error").hide();
            $(".error").removeClass("error");
        });

        $(".btn-addorupdate").click(function () {
            $(".custom-format").removeAttr("disabled");
            $("#titleModal").text("Thêm mới tài khoản");
            $(".txtPassword").show();
            $(".btn-submit-format").text("Thêm mới");
            self.IsUpdate = false;
            $('#userModal').modal('show');
        })
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
       
    })
})(jQuery);