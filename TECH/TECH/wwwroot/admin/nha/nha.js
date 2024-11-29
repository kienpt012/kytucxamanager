(function ($) {
    var self = this;
    self.Data = [];
    self.UserImages = {};
    self.IsUpdate = false;  
    self.Nha = {
        Id: null,
        TenNha:"",
        MaTP:0,
        MaQH: 0,
        MaPX: 0,
        DiaChi: ""
    }

    self.Search = {
        name: "",
        loaiDV: "",
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }
    self.lstRole = [];
    self.UserSearch = {
        name: "",
        role: null,
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }
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
                html += "<td>" + item.TenNha + "</td>";
                html += "<td>" + item.MaTPStr + "</td>";
                html += "<td>" + item.MaQHStr + "</td>";
                html += "<td>" + item.MaPXStr + "</td>";
                html += "<td>" + item.DiaChi + "</td>";
                     
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
            $("#titleModal").text("Cập nhật nhà");
            $(".btn-submit-format").text("Cập nhật");
            /*$(".custom-format").attr("disabled", "disabled");*/
            self.GetById(id, self.RenderHtmlByObject);
            self.Nha.Id = id;
            $('#NhaModal').modal('show');

            self.IsUpdate = true;
        }
    }

    self.GetById = function (id, renderCallBack) {
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/Nha/GetById',
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
            tedu.confirm('Bạn có chắc muốn xóa nhà này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Nha/Delete",
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
            url: '/Admin/Nha/GetAllPaging',
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

        $('.add-role').click(function () {
            $('#AddRole').modal('show');
        })

        //$('body').on('click', '.btn-delete', function () {
        //    var id = $(this).attr('data-id');
        //    var fullname = $(this).attr('data-fullname');
        //    if (id !== null && id !== '') {
        //        self.confirmUser(fullname, id);
        //    }
        //})
        //$(".add-image").click(function () {
        //    $("#file-input").click();
        //})

        //$('body').on('click', '.btn-role-user', function () {
        //    var id = $(this).attr('data-id');
        //    $("#user_id").val(id);
        //    //self.GetAllRoles(id);           
        //})

        //$('body').on('click', '.btn-set-role', function () {
        //    var userId = parseInt($("#user_id").val());
        //    $.each($("#lst-role tr"), function (key, item) {
        //        var check = $(item).find('.ckRole').prop('checked');
        //        if (check == true) {
        //            var id = parseInt($(item).find('.ckRole').val());
        //            self.lstRole.push({
        //                UserId: userId,
        //                RoleId: id
        //            });
        //        }
        //    })
        //    if (self.lstRole.length > 0) {
        //        self.SaveRoleForUser(self.lstRole, userId);
        //    }

        //})

        //$('.filesImages').on('change', function () {
        //    var fileUpload = $(this).get(0);
        //    var files = fileUpload.files;
        //    if (files != null && files.length > 0) {
        //        var fileExtension = ['jpeg', 'jpg', 'png'];
        //        var html = "";
        //        for (var i = 0; i < files.length; i++) {
        //            if ($.inArray(files[i].type.split('/')[1].toLowerCase(), fileExtension) == -1) {
        //                alert("Only formats are allowed : " + fileExtension.join(', '));
        //            }
        //            else {
        //                var src = URL.createObjectURL(files[i]);
        //                html += "<div class=\"box-item-image\"> <div class=\"image-upload item-image\" style=\"background-image:url(" + src + ")\"></div></div>";
        //            }
        //        }
        //        if (html != "") {
        //            $(".image-default").hide();
        //            $(".box-images").html(html);
        //        }
        //    }

        //});
    }


    self.AddUser = function (userView) {
        $.ajax({
            url: '/Admin/Nha/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                NhaModelView: userView
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
                    $('#NhaModal').modal('hide');
                } else {
                    tedu.notify('Tên nhà đã tồn tại', 'error');
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
            url: '/Admin/Nha/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                NhaModelView: userView
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
                    $('#NhaModal').modal('hide');
                } else {
                    tedu.notify('Tên nhà đã tồn tại', 'error');
                }
               
            }
        })
    }

    self.ValidateUser = function () {                
        $("#form-submit").validate({
            rules:
            {
                TenNha: {
                    required: true,
                },
                MaTP: {
                    required: true,
                },
                MaQH: {
                    required: true,
                },
                MaPX: {
                    required: true,
                },
                DiaChi: {
                    required: true,
                }
            },
            messages:
            {
                TenNha: {
                    required: "Tên nhà không được để trống",
                },
                MaTP: {
                    required: "Vui lòng chọn thành phố",
                },
                MaQH: {
                    required: "Vui lòng chọn quận huyện",
                },
                MaPX: {
                    required: "Vui lòng chọn phường xã",
                },
                DiaChi: {
                    required: "Vui lòng chọn địa chỉ",
                }
            },
            submitHandler: function (form) {   
                self.GetValue();
                if (self.IsUpdate) {
                    self.UpdateUser(self.Nha);
                }
                else {
                    self.AddUser(self.Nha);
                }
            }
        });
    }

    self.GetValue = function () {
        self.Nha.TenNha = $("#TenNha").val();
        self.Nha.MaTP = $("#cities").val();
        self.Nha.MaQH = $("#district").val();
        self.Nha.MaPX = $("#wards").val();     
        self.Nha.DiaChi = $("#DiaChi").val();     
    }

    Set.SetValue = function () {
        $("#TenNha").val(self.Nha.TenNha);
        $("#cities").val(self.Nha.MaTP);
        $("#district").val(self.Nha.MaQH);
        $("#wards").val(self.Nha.MaPX);     
        $("#DiaChi").val(self.Nha.DiaChi);     
    }

    self.RenderHtmlByObject = function (view) {
        $("#TenNha").val(view.TenNha);
        $("#cities").val(view.MaTP);
        $("#district").val(view.MaQH);
        $("#wards").val(view.MaPX);    
        $("#DiaChi").val(view.DiaChi);    

        //$("#cities").val(view.city_id);
        self.GetDistrictCityId(view.MaTP, view.MaQH);
        //$("#district").val(view.district_id);
        self.GetWardsDistrictId(view.MaQH, view.MaPX);
    }

    self.GetAllCities = function () {
        $.ajax({
            url: '/Admin/Nha/GetAllCity',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                var html = "<option value =\"\">Chọn thành phố</option>";
                if (response.Data != null && response.Data.length > 0) {
                    self.Cities = response.Data;
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.Id + ">" + item.Ten + "</option>";

                    }
                }
                $("#cities").html(html);
            }
        })
    }
    self.GetAllDistrict = function () {
        $.ajax({
            url: '/Admin/Nha/GetAllDistricts',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                var html = "<option value =\"\">Chọn quận/huyện </option>";
                if (response.Data != null && response.Data.length > 0) {
                    self.Districts = response.Data;
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.Id + ">" + item.Ten + "</option>";

                    }
                }
                $("#districts").html(html);
            }
        })
    }

    self.GetDistrictForCityId = function (tag) {
        var cityId = $(tag).val();
        /*self.GetDistrictCityId(cityId);*/
        $.ajax({
            url: '/Admin/Nha/GetDistrictsForCityId',
            type: 'GET',
            data: {
                cityId: cityId
            },
            dataType: 'json',
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {

                var html = "<option value=\"\">Chọn quận/huyện</option>";
                if (response.Data != null && response.Data.length > 0) {
                    $("#district").removeAttr("disabled");
                    self.Districts = response.Data;
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.Id + ">" + item.Ten + "</option>";
                    }
                }
                else {
                }
                $("#district").html(html);
            }
        })
    }
    self.GetDistrictCityId = function (cityId, districtId) {
        $.ajax({
            url: '/Admin/Nha/GetDistrictsForCityId',
            type: 'GET',
            data: {
                cityId: cityId
            },
            dataType: 'json',
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {

                var html = "<option value=\"\">Chọn quận/huyện</option>";
                if (response.Data != null && response.Data.length > 0) {
                    $("#district").removeAttr("disabled");
                    self.Districts = response.Data;
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        if (item.Id == districtId) {
                            html += "<option value =" + item.Id + " selected>" + item.Ten + "</option>";
                        }
                        else {
                            html += "<option value =" + item.Id + ">" + item.Ten + "</option>";
                        }

                    }
                }
                else {
                    //$("#wards").attr("disabled","disabled");
                    //$("#district").attr("disabled", "disabled");
                }
                $("#district").html(html);
            }
        })
    }


    self.GetWardsForDistrictId = function (tag) {
        var districtId = $(tag).val();
        $.ajax({
            url: '/Admin/Nha/GetWardsForDistrictId',
            type: 'GET',
            data: {
                districtId: districtId
            },
            dataType: 'json',
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                var html = "<option value=\"\">Chọn phường/xã</option>";
                if (response.Data != null && response.Data.length > 0) {
                    /*  $("#wards").removeAttr("disabled");*/
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.Id + ">" + item.Ten + "</option>";

                    }
                }
                else {
                    /*  $("#wards").attr("disabled", "disabled");*/
                }
                $("#wards").html(html);
            }
        })
    }

    self.GetWardsDistrictId = function (districtId, wardsId) {
        $.ajax({
            url: '/Admin/Nha/GetWardsForDistrictId',
            type: 'GET',
            data: {
                districtId: districtId
            },
            dataType: 'json',
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                var html = "<option value=\"\">Chọn phường/xã</option>";
                if (response.Data != null && response.Data.length > 0) {
                    /*  $("#wards").removeAttr("disabled");*/
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        if (item.Id == wardsId) {
                            html += "<option value =" + item.Id + " selected>" + item.Ten + "</option>";
                        }
                        else {
                            html += "<option value =" + item.Id + ">" + item.Ten + "</option>";
                        }


                    }
                }
                else {
                    /*  $("#wards").attr("disabled", "disabled");*/
                }
                $("#wards").html(html);
            }
        })
    }


    self.GetAllWards = function () {
        $.ajax({
            url: '/Admin/Nha/GetAllWards',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                var html = "<option value =\"\">Chọn phường/xã </option>";
                if (response.Data != null && response.Data.length > 0) {
                    self.Wards = response.Data;
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.Id + ">" + item.Ten + "</option>";

                    }
                }
                $("#wards").html(html);
            }
        })
    }

    $(document).ready(function () {
        self.GetDataPaging();
        self.ValidateUser();    
        self.GetAllCities();

        $(".modal").on("hidden.bs.modal", function () {
            $(this).find('form').trigger('reset');
            $("form").validate().resetForm();
            $("label.error").hide();
            $(".error").removeClass("error");
        });

        $(".btn-addorupdate").click(function () {
            //$(".custom-format").removeAttr("disabled");
            $("#titleModal").text("Cập nhật nhà");
            //$(".txtPassword").show();
            $(".btn-submit-format").text("Thêm mới");
            self.IsUpdate = false;
            $('#NhaModal').modal('show');
        })
        
        $('#select-right').on('change', function () {
            $('input.form-search').val("");
            /*self.Nha.name = null;*/
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