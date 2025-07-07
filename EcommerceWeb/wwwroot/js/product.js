let dataTable;

$(document).ready(function () {
    loadDataTable()
})

function loadDataTable(){
    dataTable = $('#dataTable').DataTable({
        "ajax": "/admin/product/GetAll",
        "columns": [
            { "data": "title" },
            { "data": "isbn" },
            { "data": "listPrice" },
            {"data": "author" },
            {"data": "category.name" },
            {
                "data": "id",
                "render": function (data, type, row) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i> Edit</a>
                        <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger mx-2"><i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                }
            },
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (result) {
                    dataTable.ajax.reload();
                    toastr.success(result.message);
                }
            })
        }
    });
}