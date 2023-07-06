function loadDataTable() {
    datatable = $('#prodTable').DataTable({
        "ajax": {
            url: '/Admin/Product/getall'
        },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'title', "width": "25%" },
            { data: 'category.name', "width": "25%" },
            { data: 'price', "width": "15%" },
            { data: 'count_In_Stock', "width": "10%" },
            { data: 'rating', "width": "10%" },
            {
                data: 'id', 
                render: function (data) {
                    return `<div class='btn-group'>
                        <a href="/admin/product/upsert?id=${data}"
                            class = 'btn btn-warning'>Edit</a>
                        <a href="/admin/product/deleteproduct?id=${data}"
                            class='btn btn-danger'>Delete</a>

                    </div>`
                },
                width: "10%"
            }
        ]
    });
}

$(document).ready(function () {
    loadDataTable();
})