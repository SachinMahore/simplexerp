$(document).ready(function () {
    currentMontYear();
    ddlGetMaintenance();
    GetMaintenance();
    $("#file1").on("change", function () {
        var file = this.files[0];
        var allowedTypes = ["image/jpeg", "image/png", "image/jpg"];
        var maxSize = 1 * 1024 * 1024; // 1MB

        if (file) {
            if (!allowedTypes.includes(file.type)) {
                $("#fileError").text("Only JPG and PNG images are allowed.").show();
                this.value = ""; // Clear the input
            } else if (file.size > maxSize) {
                $("#fileError").text("Image must be less than 1MB.").show();
                this.value = ""; // Clear the input
            } else {
                $("#fileError").hide(); // Valid file
            }
        }
    });
});

var SaveMaintenace = function () {
    $formData = new FormData();
    var Image = document.getElementById('file1');
    if (Image.files.length > 0) {
        for (var i = 0; i < Image.files.length; i++) {
            var file = Image.files[i];
            var fileType = file.type;
            var fileName = file.name.toLowerCase();

            // Check MIME type or file extension
            if (fileType === "image/jpeg" || fileType === "image/png" ||
                fileName.endsWith(".jpg") || fileName.endsWith(".jpeg") || fileName.endsWith(".png")) {
                $formData.append('file1-' + i, file);
            } else {
                alert("Only JPG and PNG files are allowed. File skipped: " + file.name);
                return;
            }
        }
    }
    else {
        alert('Receipt is required.');
        return;
    }
    var id = $("#hdnId").val();
    var flatno = $("#txtFlatNo").val();
    var amount = $("#txtAmount").val();
    var transaction = $("#txtTransaction").val();
    var paymentmode = $("#txtPaymentMode").val();
    var month = $("#txtMonth").val();
    var year = $("#txtYear").val();
    var receipt = $("#txtReceipt").val();
    var paiddate = $("#txtPaidDate").val();

    // Validate Flat No
    if (flatno === "" || flatno === "1") {
        alert('Flat No is required.');
        $("#txtFlatNo").focus();
        return;
    }

    // Validate Amount
    if (amount === "" || isNaN(amount) || parseFloat(amount) <= 0) {
        alert('Enter a valid amount.');
        $("#txtAmount").focus();
        return;
    }

    // Validate Transaction ID
    if (transaction === "") {
        alert('Transaction no. is required.');
        $("#txtTransaction").focus();
        return;
    }
    if (paiddate === "") {
        alert('Paid date. is required.');
        $("#txtPaidDate").focus();
        return;
    }

    $formData.append('Id', id);
    $formData.append('FlatNo', flatno);
    $formData.append('Amount', amount);
    $formData.append('TransactionNo', transaction);
    $formData.append('PaymentMode', paymentmode);
    $formData.append('Month', month);
    $formData.append('Year', year);
    $formData.append('Receipt', receipt);
    $formData.append('PaidDate', paiddate);

    $.ajax({
        url: "/Maintenance/SaveMaintenance",
        method: "Post",
        data: $formData,
        contentType: "application/json;charset=utf-8",
        contentType: false,
        processData: false,
        async: false,
        success: function (response) {
            alert(response.Message);
            //GetMaintenance();
        },
        error: function (response) {
            alert(response.Message);
        }
    });
}

var GetMaintenance = function () {
    var flatno = $("#hdnFlatNo").val();
    $.ajax({
        url: "/Maintenance/GetMaintenance?Year=" + $("#txtYear1").val() + "&month=" + $("#txtMonth1").val() + "&flatNo=" + $("#ddlFlatNo").val(),
        method: "post",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {

            $("#tblMaintenance tbody").empty();
            $("#maintenanceCards").empty();

            var amount = 0;
            var html = "";

            $.each(response.Message, function (index, item) {
                amount += parseInt(item.Amount || 0);

                // Table Row
                html += `<tr>
        <td>${item.FlatNo}</td>
        <td>${item.Month}</td>
        <td>${item.Year}</td>
        <td>${item.Amount}</td>
        <td>${item.PaymentMode}</td>
        <td>${item.TransactionNo}</td>
        <td>${item.PaidDate}</td>
        <td><input type='submit' class='btn btn-success' value='Edit' ${(flatno === '104' || flatno === '301') ? `onclick="EditMaintenance(${item.Id})"` : "disabled"} /></td>
    </tr>`;

                // Card View
                var card = `
        <div class="col-md-3 mb-4">
            <div class="card h-100 shadow-sm">
                <img src="../Content/Img/Maintenance/${item.Receipt}" class="card-img-top" style="height: 320px; object-fit: cover;" />
                <div class="card-body">
                    <h5 class="card-title">Flat: ${item.FlatNo}                          <button class="btn btn-success btn-sm" ${(flatno === '104' || flatno === '301') ? `onclick="EditMaintenance(${item.Id})"` : "disabled"}>Edit</button></h5>

                    <p class="card-text">
                        <strong>MM/YY:</strong> ${item.Month}-${item.Year}<br/>
                        <strong>Amt:</strong> ₹${item.Amount}
                        <strong>Mode:</strong> ${item.PaymentMode}<br/>
                        <strong>Txn No:</strong> ${item.TransactionNo}<br/>
                        <strong>Paid:</strong> ${item.PaidDate}<br/>
                                           </p>
                </div>
 
            </div>
        </div>`;
              
                $("#maintenanceCards").append(card);
            });
            $("#tblMaintenance").append(html);


            $("#lblamount").text("Total Amount = " + amount);
            $("#lblPaidamount2").text(amount + "/-");
        }

    });

}

function showTable() {
    $("#tblMaintenance").show();
    $("#maintenanceCards").hide();
}

function showCards() {
    $("#tblMaintenance").hide();
    $("#maintenanceCards").show();
}

var ddlGetMaintenance = function () {

    $.ajax({
        url: "/Resident/GetResident",
        method: "post",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var html = "";
/*            $("#tblMaintenance tbody").empty();*/
            html += "<option value='1' selected>All Flat </option>";

            $.each(response.message, function (MaintenanceIndex, elementValue) {
                html += "<option value='" + elementValue.FlatNo + "'>" + elementValue.FlatNo + "</option>";

            });
            $("#ddlFlatNo").append(html);
        }

    });

}

var DeleteMaintenance = function (Id) {

    var model = { Id: Id };
    $.ajax({
        url: "/Maintenance/DeleteMaintenance",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            alert("Record Deleted Successfully");
            GetMaintenance();
        }
    });
}

var EditMaintenance = function (Id) {

    var model = { Id: Id };
    $.ajax({
        url: "/Maintenance/EditMaintenance",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {


            $("#hdnId").val(response.model.Id);
            $("#txtMonth").val(response.model.Month);
            $("#txtYear").val(response.model.Year);
            $("#txtFlatNo").val(response.model.FlatNo);
            $("#txtTransaction").val(response.model.TransactionNo);
            $("#txtAmount").val(response.model.Amount);
            //$("#txtReceipt").val(response.model.Receipt);
            $("#txtPaymentMode").val(response.model.PaymentMode);
            $("#txtPaidDate").val(formatDate(response.model.PaidDate));

        }
    });
}
var MonthChangeEvent = function () {
    $("#txtYear1").val("");
    //GetMaintenance();
}
var yearchangeenven = function () {

    if ($("#txtMonth1").val() == "") {
        alert("Please Select Month!");
/*        $("#txtYear1").val("");*/
        return false();
    }

   // GetMaintenance();
}
var currentMontYear = function () {
    var currentDate = new Date();
    $("#txtYear").val(currentDate.getFullYear());
    $("#txtYear1").val(currentDate.getFullYear());
    var currentMonth;
    switch (currentDate.getMonth()) {
        case 0: currentMonth = "January";
            break;
        case 1: currentMonth = "February";
            break;
        case 2: currentMonth = "March";
            break;
        case 3: currentMonth = "April";
            break;
        case 4: currentMonth = "May";
            break;
        case 5: currentMonth = "June";
            break;
        case 6: currentMonth = "July";
            break;
        case 7: currentMonth = "August";
            break;
        case 8: currentMonth = "September";
            break;
        case 9: currentMonth = "October";
            break;
        case 10: currentMonth = "November";
            break;
        case 11: currentMonth = "December";
            break;
    }
    $("#txtMonth").val(currentMonth);
    $("#txtMonth1").val(currentMonth);
}
function formatDate(dateStr) {
    let parts = dateStr.split("/"); // Assuming input format is "dd/MM/yyyy"

    let day = parts[1].padStart(2, '0');   // Ensure two-digit day
    let month = parts[0].padStart(2, '0'); // Ensure two-digit month
    let year = parts[2];  // Year remains unchanged

    return `${year}-${month}-${day}`;  // Convert to "yyyy-MM-dd"
}

