﻿(function ($) {
    var self = this;
    self.Data = [];
    self.UserImages = {};
    self.IsUpdate = false;  
    self.TheSinhVien = {
        Id: null,
        UserId:0,
        MaThe: "",
        NgayTaoThe: "",
        NgayHetHan: "",
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
                html += "<td>" +(item.KhachHang != null ? item.KhachHang.TenKH:"") + "</td>";
                html += "<td>" + item.MaThe + "</td>";
                html += "<td>" + item.NgayTaoTheStr + "</td>";
                html += "<td>" + item.NgayHetHanStr + "</td>";
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
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.Id + ">" + item.TenKH + "</option>";
                    }
                }
                $("#UserId").html(html);
            }
        })
    }


    self.Update = function (id) {
        if (id != null && id != "") {
            $(".txtPassword").hide();
            $("#titleModal").text("Cập nhật thẻ sinh viên");
            $(".btn-submit-format").text("Cập nhật");
            /*$(".custom-format").attr("disabled", "disabled");*/
            self.GetById(id, self.RenderHtmlByObject);
            self.TheSinhVien.Id = id;
            $('#userModal').modal('show');

            self.IsUpdate = true;
        }
    }

    self.GetById = function (id, renderCallBack) {
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/TheSinhVien/GetById',
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
            tedu.confirm('Bạn có chắc muốn xóa thẻ sinh viên này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/TheSinhVien/Delete",
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
        //var _data = {
        //    Name: $(".name-search").val() != "" ? $(".name-search").val() : null,
        //    loaiDV: $("#LoaiDV").val() != "" ? $("#LoaiDV").val() : null,
        //    PageIndex: tedu.configs.pageIndex,
        //    PageSize: tedu.configs.pageSize
        //};

        self.UserSearch.PageIndex = tedu.configs.pageIndex;
        self.UserSearch.PageSize = tedu.configs.pageSize;

        $.ajax({
            url: '/Admin/TheSinhVien/GetAllPaging',
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
            self.TheSinhVien.Id = 0;
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
            url: '/Admin/TheSinhVien/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                TheSinhVienModelView: userView
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
                    tedu.notify('Tên dịch vụ đã tồn tại', 'error');
                }
            }
        })
    }


    self.UpdateUser = function (userView) {
        $.ajax({
            url: '/Admin/TheSinhVien/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                TheSinhVienModelView: userView
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
                } else {
                    tedu.notify('Tên dịch vụ đã tồn tại', 'error');
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
                MaThe: {
                    required: true,
                },
                NgayTaoThe: {
                    required: true,
                },
                NgayHetHan: {
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
                    required: "Mã thẻ không được để trống",
                },
                NgayTaoThe: {
                    required: "Vui lòng chọn ngày tạo thẻ",
                }, NgayHetHan: {
                    required: "Vui lòng chọn ngày hết hạn",
                }
                ,Status: {
                    required: "Vui lòng chọn trạng thái",
                }
                
            },
            submitHandler: function (form) {   
                self.GetValue();
                if (self.IsUpdate) {
                    self.UpdateUser(self.TheSinhVien);
                }
                else {
                    self.AddUser(self.TheSinhVien);
                }
            }
        });
    }

    self.GetValue = function () {
        self.TheSinhVien.UserId = $("#UserId").val();
        self.TheSinhVien.MaThe = $("#MaThe").val();
        self.TheSinhVien.Status = $("#Status").val();
        /*self.HopDong.NgayBatDau = $("#NgayBatDau").val();   */
        self.TheSinhVien.NgayTaoThe = $.datepicker.formatDate("yy-mm-dd", $("#NgayTaoThe").datepicker("getDate"));
        self.TheSinhVien.NgayHetHan = $.datepicker.formatDate("yy-mm-dd", $("#NgayHetHan").datepicker("getDate"));
        self.TheSinhVien.Comment = $("#Comment").val();
    }

    Set.SetValue = function () {
        $("#UserId").val(self.TheSinhVien.UserId);
        $("#MaThe").val(self.TheSinhVien.MaThe);
        $("#Status").val(self.TheSinhVien.Status);

        $("#Comment").val(self.TheSinhVien.Comment);

        $("#GhiChu").val(self.TheSinhVien.GhiChu);

        $("#NgayTaoThe").val(self.TheSinhVien.NgayTaoThe);
        $("#NgayHetHan").val(self.TheSinhVien.NgayHetHan);
    }

    self.RenderHtmlByObject = function (view) {

        $("#UserId").val(view.UserId);
        $("#MaThe").val(view.MaThe);
        $("#Status").val(view.Status);

        $("#Comment").val(view.Comment);

        $("#GhiChu").val(view.GhiChu);

        $("#NgayTaoThe").datepicker("setDate", new Date(view.NgayTaoTheStr));
        $("#NgayHetHan").datepicker("setDate", new Date(view.NgayHetHanStr));
       
    }







    //self.GetAllRole = function () {
    //    $.ajax({
    //        type: "GET",
    //        url: "/Admin/DichVu/GetAll",
    //        dataType: "json",
    //        beforeSend: function () {
    //        },
    //        complete: function () {
    //        },
    //        success: function (response) {
    //            if (response.Data !== null && response.Data.length > 0) {
    //                self.BindRoleHtml(response.Data)
    //            }
    //        },
    //        error: function () {
    //        }
    //    });
    //}    
    //self.BindRoleHtml = function (data) {
    //    if (data !== null && data.length > 0) {
    //        var html = "<option value='0'>Chọn quyền</option>";
    //        $.each(data, function (key, item) {
    //            html += "<option value=" + item.Id + ">" + item.Name+"</option>";
    //        })
    //        $(".data-select2").html(html);
    //    }
    //}

    $(document).ready(function () {
        self.GetDataPaging();
        self.ValidateUser();    
        self.GetAllKhachHang();
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