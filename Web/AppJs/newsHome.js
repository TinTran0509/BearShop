var pageIndex = 1;
var pageSize = 20;
var News = function () {
    return {
        init: function () {
            News.loadData();
        },
        loadData: function () {
            let linkseo = $('#linkseo').val();
            $.get("/News/LoadData", { linkseo: linkseo, pageIndex: pageIndex, pageSize: pageSize }, function (res) {
                $('#loadData').html(res.viewContent);
                $('#titleCate').text(res.categoryNews);
                $('#pathway').text(res.categoryNews);
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
        }
    }
}();
$(function () {
    News.init();
})