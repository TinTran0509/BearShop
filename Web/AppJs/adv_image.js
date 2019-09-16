var pageIndex = 1;
var Adv_Image = function () {
    return {
        init: function () {
            Adv_Image.loadData();
        },
        loadData: function () {
            $.get("/Admin/AdvImage/ListData", { pageIndex: pageIndex}, function (res) {
                $('#loadData').html(res.viewContent);
                if (res.totalPages > 1) {
                    $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                    $('#pagination').twbsPagination({
                        startPage: self.pageIndex,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            self.pageIndex = page;
                            Adv_Image.loadData(self.pageIndex);
                        }
                    });
                } else {
                    $('#paginationholder').html('');
                }
            });
        },
        handleAddItem: function () {
            modal.Render("/Admin/AdvImage/Add", "Thêm mới ảnh quảng cáo", "modal-lg");
        },
        handleEditItem: function (id) {
            modal.Render("/Admin/AdvImage/Edit/" + id, "Cập nhật ảnh quảng cáo", "modal-lg");
        },
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Message);
                Adv_Image.loadData();
                modal.hide();
            }
            else {
                alertmsg.error(res.Message);
            }
        },
        onEditSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Message);
                Adv_Image.loadData();
                modal.hide();
            }
            else {
                alertmsg.error(res.Message);
            }
        },
        handleDeleteItem: function (id) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có chắc chắn xóa không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/AdvImage/Delete", { id: id }, function (res) {
                        self.pageIndex = 1;
                        //self.loadData(self.pageIndex);
                        Adv_Image.loadData();
                        alertmsg.success(res.Message);
                        modal.hide();
                    });
                }
                $("#loading").hide();
            });
        }
    }
}();
$(function () {
    Adv_Image.init();
})