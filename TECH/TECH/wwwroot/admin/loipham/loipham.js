(function ($) {
    var self = this;
    self.DataLoi = [];
    self.IsUpdateLoi = false;
    self.LoiPham = {
        Id: 0,
        TenLoi: "",
        TienPhat: 0,
        GhiChu:""
    }
    self.UpdateLoi = function (id) {
        if (id != null && id != "") {
            $("#titleModal").text("Cập nhật lỗi phạm");
            $(".btn-submit-format").text("Cập nhật");
            self.GetByIdLoi(id);   
            self.LoiPham.Id = id;
            $('#categoryModal').modal('show');

            self.IsUpdateLoi = true;
        }
    }

    self.GetByIdLoi = function (id) {
        //self.userData = {};
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/LoiPham/GetById',
                type: 'GET',
                dataType: 'json',
                data: {
                    id: id
                },
                beforeSend: function () {
                    //Loading('show');
                },
                complete: function () {
                    ////Loading('hiden');
                },
                success: function (response) {
                    if (response.Data != null) {
                        $("#TenLoi").val(response.Data.TenLoi);
                        $("#TienPhat").val(response.Data.TienPhat);
                        $("#GhiChuLoi").val(response.Data.GhiChu);

                        self.LoiPham.Id = id;
                        $("#loiPhamModal").modal('show');

                    }
                }
            })
        }
    }
    self.DeletedLoi = function (id) {
        if (id != null && id != "") {
            tedu.confirm('Bạn có chắc muốn xóa lỗi phạm này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/LoiPham/Delete",
                    data: { id: id },
                    beforeSend: function () {
                        // tedu.start//Loading();
                    },
                    success: function () {
                        tedu.notify('Đã xóa thành công', 'success');
                        //tedu.stop//Loading();
                        //loadData();
                        //self.GetDataPaging(true);
                        wi
                    },
                    error: function () {
                        tedu.notify('Has an error', 'error');
                        tedu.stop//Loading();
                    }
                });
            });
        }
    }

    self.GetUrlParameter = function getUrlParameter(sParam) {
        var sPageURL = window.location.search.substring(1),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;

        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? "" : decodeURIComponent(sParameterName[1]);
            }
        }
        return "";
    };
   

    self.AddLoi = function (userView) {
        var mahoadon = self.GetUrlParameter('mahoadon');
        if (mahoadon != "") {
            mahoadon = parseInt(mahoadon);
            $.ajax({
                url: '/Admin/ChiTietHoaDon/AddLoiPhamAndAddLoiChiTietHoaDon',
                type: 'POST',
                dataType: 'json',
                data: {
                    loiPhamModelView: userView,
                    maHoaDon: mahoadon
                },
                beforeSend: function () {
                    //Loading('show');
                },
                complete: function () {
                    //Loading('hiden');
                },
                success: function (response) {
                    if (response.success) {
                        tedu.notify('Thêm mới lỗi thành công', 'success');
                        $('#loiPhamModal').modal('hide');
                        window.location.reload();
                    }
                }
            })
        }
      
    }
    self.UpdateObjectLoi = function (userView) {
        if (self.GetUrlParameter('mahoadon') !="") {
            $.ajax({
                url: '/Admin/Category/Update',
                type: 'POST',
                dataType: 'json',
                data: {
                    loiPhamModelView: userView
                },
                beforeSend: function () {
                    //Loading('show');
                },
                complete: function () {
                    //Loading('hiden');
                },
                success: function (response) {
                    if (response.success) {
                        /*self.GetDataPaging(true);*/
                        $('#loiPhamModal').modal('hide');
                        window.location.reload();
                    }

                }
            })
        }
    }

    self.ValidateUserLoi = function () {
        $("#form-submit-loi").validate({
            rules:
            {
                TenLoi: {
                    required: true,
                },
                TienPhat: {
                    required: true,
                }                            
            },
            messages:
            {
                TenLoi: {
                    required: "Vui lòng nhập tên lỗi",
                },
                TienPhat: {
                    required: "Vui lòng nhập tiền phạt",
                } 
            },
            submitHandler: function (form) {
                self.GetValueLoi();               
                if (self.IsUpdateLoi) {
                    self.UpdateObjectLoi(self.LoiPham);
                }
                else {
                    self.AddLoi(self.LoiPham);
                }
            }
        });
    }

    self.GetValueLoi = function () {
        self.LoiPham.TenLoi = $("#TenLoi").val();
        self.LoiPham.TienPhat = $("#TienPhat").val();
        self.LoiPham.GhiChu = $("#GhiChuLoi").val();
    }
   
    $(document).ready(function () {
        self.ValidateUserLoi();
        
        $(".modal").on("hidden.bs.modal", function () {
            $(this).find('form').trigger('reset');
            $("form").validate().resetForm();
            $("label.error").hide();
            $(".error").removeClass("error");
            self.IsUpdateLoi = false;
        });

        $(".btn-addloi").click(function () {
            $("#titleModal").text("Thêm mới danh mục");
            $(".btn-submit-format-loi").text("Thêm mới");
            self.IsUpdateLoi = false;
        })
    })
})(jQuery);