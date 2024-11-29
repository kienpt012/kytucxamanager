(function ($) {
    var self = this;
    self.Data = [];
    self.UserImages = {};
    self.IsUpdate = false;  
    self.SuaChua = {
        Id: null,
        UserId:0,
        MaPhong: "",
        NgayTao: "",
        Status: 0,
        Comment: "",
    }
    self.Search = {
        name: "",
        loaiDV: "",
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
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
                html += "<td>" + (item.KhachHang != null ? item.KhachHang.TenKH : "") + "</td>";
                html += "<td>" + (item.Phong != null ? item.Phong.TenPhong : "") + "</td>";
                html += "<td>" + item.NgayTaoStr + "</td>";
                html += "<td>" + item.StatusStr + "</td>";
                html += "<td>" + item.Comment + "</td>";
                     
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
                var html = "<option value =\"\">Chọn Sinh Vien</option>";
                if (response.Data != null && response.Data.length > 0) {
                    var userId = $("#userIdPhong").val();
                    if (userId != null && userId != "" && userId > 0) {
                        var data = response.Data.find(p => p.Id == parseInt(userId));
                        html += "<option value =" + parseInt(userId) + " selected ='true'>" + data.TenKH + "</option>";
                    } else {
                        for (var i = 0; i < response.Data.length; i++) {
                            var item = response.Data[i];
                            html += "<option value =" + item.Id + ">" + item.TenKH + "</option>";
                        }
                    }
                    
                }
                $("#UserId").html(html);
            }
        })
    }


    self.Update = function (id) {
        if (id != null && id != "") {
            $(".txtPassword").hide();
            $("#titleModal").text("Cập nhật yêu cầu sửa chữa");
            $(".btn-submit-format").text("Cập nhật");
            /*$(".custom-format").attr("disabled", "disabled");*/
            self.GetById(id, self.RenderHtmlByObject);
            self.SuaChua.Id = id;
            $('#userModal').modal('show');

            self.IsUpdate = true;
        }
    }

    self.GetById = function (id, renderCallBack) {
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/SuaChua/GetById',
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
            tedu.confirm('Bạn có chắc muốn xóa yêu cầu sửa chữa này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/SuaChua/Delete",
                    data: { id: id },
                    beforeSend: function () {
                    },
                    success: function () {
                        tedu.notify('Đã xóa thành công', 'success');
                        self.GetDataPaging(true);
                        window.location.reload();
                    },
                    error: function () {
                        tedu.notify('Has an error', 'error');
                    }
                });
            });
        }
    }

    self.GetDataPaging = function (isPageChanged) {
        //var _data = {
        //    Name: $(".name-search").val() != "" ? $(".name-search").val() : null,
        //    loaiDV: $("#LoaiDV").val() != "" ? $("#LoaiDV").val() : null,
        //    PageIndex: tedu.configs.pageIndex,
        //    PageSize: tedu.configs.pageSize
        //};

        self.UserSearch.PageIndex = tedu.configs.pageIndex;
        self.UserSearch.PageSize = tedu.configs.pageSize;

        $.ajax({
            url: '/Admin/SuaChua/GetAllPaging',
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
            self.SuaChua.Id = 0;
            self.IsUpdate = false;
            $('#CreateOrUpdate').modal('show')
        })

        // hủy add và edit
        $(".cs-close-addedit,.btn-cancel-addedit").click(function () {
            $("#CreateEdit").css("display", "none");
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

   

    self.AddUser = function (userView) {
        $.ajax({
            url: '/Admin/SuaChua/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                SuaChuaModelView: userView
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
                    window.location.reload();
                }
                
            }
        })
    }


    self.UpdateUser = function (userView) {
        $.ajax({
            url: '/Admin/SuaChua/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                SuaChuaModelView: userView
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
                    window.location.reload();
                }
               
            }
        })
    }

    self.ValidateUser = function () {                
        $("#form-submit").validate({
            rules:
            {
                UserId: {
                    required: true,
                },
                MaPhong: {
                    required: true,
                },
                Status: {
                    required: true,
                }
            },
            messages:
            {
                UserId: {
                    required: "Tên sinh viên không được để trống",
                },
                MaThe: {
                    required: "Mã phòng không được để trống",
                }
                ,Status: {
                    required: "Vui lòng chọn trạng thái",
                }
                
            },
            submitHandler: function (form) {   
                self.GetValue();
                if (self.IsUpdate) {
                    self.UpdateUser(self.SuaChua);
                }
                else {
                    self.AddUser(self.SuaChua);
                }
            }
        });
    }

    self.GetValue = function () {
        self.SuaChua.UserId = $("#UserId").val();
        self.SuaChua.MaPhong = $("#MaPhong").val();
        self.SuaChua.Status = $("#Status").val();
        /*self.HopDong.NgayBatDau = $("#NgayBatDau").val();   */
        self.SuaChua.Comment = $("#Comment").val();
    }

    Set.SetValue = function () {
        $("#UserId").val(self.SuaChua.UserId);
        $("#MaPhong").val(self.SuaChua.MaPhong);
        $("#Status").val(self.SuaChua.Status);

        $("#Comment").val(self.SuaChua.Comment);
    }

    self.RenderHtmlByObject = function (view) {

        $("#UserId").val(view.UserId);
        $("#MaPhong").val(view.MaPhong);
        $("#Status").val(view.Status);

        $("#Comment").val(view.Comment);

       
    }

    self.GetAllPhong = function () {
        $.ajax({
            url: '/Admin/Phong/GetAll',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
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

    $(document).ready(function () {
        self.GetDataPaging();
        self.ValidateUser();    
        self.GetAllKhachHang();
        self.GetAllPhong();
        $(".modal").on("hidden.bs.modal", function () {
            $(this).find('form').trigger('reset');
            $("form").validate().resetForm();
            $("label.error").hide();
            $(".error").removeClass("error");
        });


        $(".formatdate").datepicker({
            dateFormat: 'dd/mm/yy'
        });

        //$(".modal").on("hidden.bs.modal", function () {
        //    $(this).find('form').trigger('reset');
        //    $("form").validate().resetForm();
        //    $("label.error").hide();
        //    self.IsUpdate = false;
        //    $("#titleModal").text("Thêm mới hợp đồng");
        //    $(".btn-submit-format").text("Thêm mới");
        //    self.HopDong.Id = 0;
        //    $(".error").removeClass("error");
        //});
        
        $('#select-right').on('change', function () {
            $('input.form-search').val("");
            /*self.DichVu.name = null;*/
            self.Search.loaiDV = $(this).val();
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