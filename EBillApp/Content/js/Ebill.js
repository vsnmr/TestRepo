

$(document).ready(function () {

    $('#addItem').click(function () {

    
        let ProductName = $('#productname').val();
        let Price = $('#price').val();
        let Quantity = $('#quantity').val();
        let ItemIndex = $('#items tbody tr').length;
        
        $.ajax({

            url: "/EBill/CreateItem",
            type: 'Post',
            data: {
                ProductName: ProductName, Price: Price, Quantity: Quantity, ItemIndex: ItemIndex
            },

            success: function (resp) {

                $('#items tbody').append(resp);

                $('#productname').val("");
                $('#price').val("");
                $('#quantity').val("");

            },
            error: function (err) {
                console.log(err)
                alert(err + "hdhdh");
            }
        })

    })

    //this mrhtod  useful when document ison ready only
    //$('.remove-btn').click(function () {
    //    alert('bye')
    //    var row = $(this).closest('tr');
    //    row.remove();
    //});
  
});

function handleClick(index) {
    $('#'+index).remove();
   
}
