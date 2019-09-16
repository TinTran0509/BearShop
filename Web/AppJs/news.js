var News = function () {
    return {
        init: function () {
            var keyWord = $('#txtSearch').val();
            if (keyWord != undefined) {
                News.loadData(1);
            }
            $("#txtSearch").keypress(function (e) {
                if (e.which === 13) {
                    News.loadData(1);
                    return false;
                }
            });
        },
        loadData: function (pageIndex) {
            var status = location.search.split('status=')[1];
            var page = localStorage.getItem("PAGE");
            if (page === null)
                page = pageIndex;
            var keyWord = $('#txtSearch').val();
            $.get("/Admin/News/ListData", { keyWord: keyWord, pageIndex: page, status: status }, function (res) {
                $('#loadData').html(res.viewContent);
                if (res.totalPages > 1) {
                    $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                    $('#pagination').twbsPagination({
                        startPage: self.pageIndex,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            self.pageIndex = page;
                            News.loadData(self.pageIndex);
                        }
                    });
                } else {
                    $('#paginationholder').html('');
                }
            });
        }, 
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Message);
                if (res.Close == "close") {
                    let status = $('#Status').val();
                    let url = "/admin/news?status=" + status;
                    location.href = url;
                } else {
                    location.href = "/admin/news/add";
                }
            }
            else {
                alertmsg.error(res.Message);
            }
        },
        onEditSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Message);
                localStorage.setItem("PAGE", res.Page);
                let status = $('#Status').val();
                let url = "/admin/news?status="+status;
                location.href = url;
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
                    AjaxService.GET("/Admin/News/Delete", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Message);
                    });
                }
                $("#loading").hide();
            });
        },
        handlePublish: function (id) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có muốn đăng bài viết?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.GET("/Admin/News/Publish/", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Message);
                    });
                }
                $("#loading").hide();
            });
        },
        handleUnPublish: function (id) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có muốn hủy đăng bài viết?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.GET("/Admin/News/UnPublish/", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Message);
                    });
                }
                $("#loading").hide();
            });
        },
        handleViewDetail: function (id) {
            modal.Render("/Admin/News/Detail/"+ id, "Chi tiết tin tức", "modal-lg");
        }
    }
}();
$(function () {
    News.init();
})