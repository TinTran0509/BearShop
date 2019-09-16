var Product = function () {
    return {
        init: function () {
            Product.loadData(1);
            $('#txtSearch').keypress(function (e) {
                if (e.which === 13) {
                    Product.loadData(1);
                    return false;
                }
            });
        },
        loadData: function (pageIndex) {
            var keyWord = $('#txtSearch').val();
            $.get("/Product/ListData", { keyWord: keyWord, pageIndex: pageIndex }, function (res) {
                $('#loadData').html(res.viewContent);
                if (res.totalPages > 1) {
                    $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                    $('#pagination').twbsPagination({
                        startPage: self.pageIndex,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            self.pageIndex = page;
                            Product.loadData(self.pageIndex);
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
    Product.init();
})