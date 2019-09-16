var feedback = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
            this.theEnd = false;
            this.wait = false;
            this.DataSearch = {};
            feedback.loadDataAdmin(this.pageIndex);
        },
        loadDataAdmin: function (pageindex) {
            var self = this;
            $("#loading").show();
            AjaxService.POST("/Admin/FeedBack/ListData?page=" + pageindex, feedback.DataSearch, function (res) {
                $("#gridData").html(res.viewContent);
                if (res.totalPages > 1) {
                    $('#paginationholder').html('');
                    $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                    $('#pagination').twbsPagination({
                        startPage: self.pageIndex,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            self.pageIndex = page;
                            feedback.loadDataAdmin(self.pageIndex);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        loadFormAnswer: function (id) {
            modal.Render("/FeedBack/Answer/" + id, "Trả lời", "modal-lg");
        },
        loadDetail: function (id) {
            modal.Render("/FeedBack/Detail/" + id, "", "modal-lg");
        },
        onAnswerSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                feedback.loadDataAdmin(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
        },
        ondelete: function (id) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có chắc chắn không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/FeedBack/Delete", { id: id }, function (res) {
                        if (res.IsSuccess) {
                            self.pageIndex = 1;
                            self.loadDataAdmin(self.pageIndex);
                            alertmsg.success(res.Message);
                        } else {
                            alertmsg.error(res.Message);
                        }
                    });
                }
                $("#loading").hide();
            });
        },
        onReply: function (id) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Xác nhận đã trả lời!",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/FeedBack/Reply", { id: id }, function (res) {
                        if (res.IsSuccess) {
                            self.pageIndex = 1;
                            self.loadDataAdmin(self.pageIndex);
                            alertmsg.success(res.Message);
                        } else {
                            alertmsg.error(res.Message);
                        }
                    });
                }
                $("#loading").hide();
            });
        },
        onmultidelete: function () {
            var self = this;
            if ($("table tbody").find("input[type=checkbox]:checked").length == 0) {
                alertmsg.error("Bạn cần chọn ít nhất một câu hỏi cần xóa");
            } else {
                $("#loading").show();
                swal({
                    title: "Bạn có chắc chắn không?",
                    text: "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Có",
                    cancelButtonText: "không",
                }, function (isConfirm) {
                    if (isConfirm) {
                        AjaxService.POST("/FeedBack/DeleteAll", { lstid: $("#hdfID").val() }, function (res) {
                            self.pageIndex = 1;
                            self.loadDataAdmin(self.pageIndex);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            }
        },
    }
}();
$(function () { feedback.init(); });


