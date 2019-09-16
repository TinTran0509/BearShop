var video = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            $('#Name').keypress(function (e) {
                if (e.which === 13) {
                    video.loadData(self.pageIndex);
                    return false;
                }
            });
            $('#btnSearch').click(function () {
                video.loadData(self.pageIndex);
            })
        },
        loadData: function (pageindex) {
            var self = this;
            if (pageindex == undefined)
                pageindex = self.pageIndex;
            let name =  $('#Name').val();
            $("#loading").show();
            AjaxService.GET("/video/ListData", { name:name,page: pageindex }, function (res) {
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
                            video.loadData(self.pageIndex);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                video.loadData(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            modal.hide();
            $("#loading").hide();
        },
        jYoutube: function (url, size) {
            if (url === null) { return ""; }

            size = (size === null) ? "big" : size;
            var vid;
            var results;

            results = url.match("[\?&]v=([^&#]*)");

            vid = (results === null) ? url : results[1];

            if (size == "small") {
                return "http://img.youtube.com/vi/" + vid + "/2.jpg";
            } else {
                return "http://img.youtube.com/vi/" + vid + "/0.jpg";
            }
        }
    };
}();
$(function () { video.init(); });
