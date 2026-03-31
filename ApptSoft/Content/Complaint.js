$(document).ready(function () {
   // ddlComplaintFlatno();
    GetComplaint();
    GetDashboardComplaint();
    GetComplaintDash();
});


var AddComplaint = function () {
    $("#AddComplaintModal").modal('show');
}
var SaveComplaint = function () {
    debugger;

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
    var id = $("#hdnid").val();
    var flatno = $("#txtFlatNo").val();
    var title = $("#txtTitle").val();
    var description = $("#txtDiscription").val();
    var complaint = $("#txtComplaint").val();
    var complaintdate = $("#txtComplaintDate").val();
    var resolvedby = $("#txtResolvedBy").val();
    var resolveddate = $("#txtResolvedDate").val();
    var proof = $("#file1").val();
    if (flatno === "1") {
        alert("Flat No is required.");
        $("#txtFlatNo").focus();
        return;
    }
    if (title === "") {
        alert("Title is required.");
        $("#txtTitle").focus();
        return;
    }
    if (complaint === "") {
        alert("Complaint is required.");
        $("#txtComplaint").focus();
        return;
    }

    $formData.append('Id', id);
    $formData.append('FlatNo', flatno);
    $formData.append('Title', title);
    $formData.append('Description', description);
    $formData.append('Complaint', complaint);
    $formData.append('ComplaintDate', complaintdate);
    $formData.append('ResolvedBy', resolvedby);
    $formData.append('ResolveDate', resolveddate);
    $formData.append('Proof', proof);
    $formData.append('Status', '0');

    $.ajax({
        url: "/Complaint/SaveComplaint",
        method: "Post",
        data: $formData,
        contentType: "application/json;charset=utf-8",
        contentType: false,
        processData: false,
        async: false,
        success: function (response) {
            alert(response.Message)
            GetComplaint();
        },
        error: function (response) {
            alert(response.model);
        }
    });

}

//ListCode

var GetComplaintDash = function () {
    debugger;
    $.ajax({
        url: "/Complaint/GetComplaint",
        method: "post",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        async: false,
        success: function (response) {
            var html = "";

            $("#tblComplaints1 tbody").empty();
            $.each(response.model, function (index, elementValue) {
                html += "<tr><td>" + elementValue.FlatNo +
                    //"</td><td>" + elementValue.Id +
                    "</td><td>" + elementValue.Title + "</td ></tr > ";


            });
            $("#getcom tbody").append(html);


        }
    });
}

var GetComplaint = function () {
    debugger;
    $.ajax({
        url: "/Complaint/GetComplaint",
        method: "post",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        async: false,
        success: function (response) {
            var html = "";

            $("#tblComplaints tbody").empty();
            $.each(response.model, function (index, elementValue) {
                html += "<tr><td>" + "<span onclick=EditComplaint(" + elementValue.Id + ")>" + elementValue.FlatNo + "</span>" +
                    "</td><td>" + elementValue.Title +
                    "</td><td>" + elementValue.Complaint +
                    "</td><td>" + elementValue.ComplaintDate +
                    "</td><td>" + (elementValue.Status ? "Resolved": '<span style="color: red;">Pending</span>') +
                    "</td><td>" + elementValue.ResolvedBy +
                    "</td><td>" + elementValue.ResolveDate +
                    "</td><td><button type='button' class='btn btn-warning' value='Detail' onclick='ComplaintDetail(" + elementValue.Id + ")'><i class='bi bi-eye-fill'></i></button></td></tr>";


            });
            $("#tblComplaints tbody").append(html);


        }
    });
}


//DeleteCode
var DeleteComplaint = function (Id) {
    var model = { Id: Id };
    $.ajax({
        url: "/Complaint/DeleteComplaint",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            alert("Record Deleted Successfully");
            GetComplaint();
        }
    });
}


//EditCode
var EditComplaint = function (Id) {
    var model = { Id: Id };

    $.ajax({
        url: "/Complaint/ComplaintDetail",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $("#AddComplaintModal").modal('show');

            $("#hdnid").val(response.model.Id);
            $("#txtFlatNo").val(response.model.FlatNo);
            $("#txtTitle").val(response.model.Title);
            $("#txtDiscription").val(response.model.Description);
            $("#txtComplaint").val(response.model.Complaint);
            $("#txtComplaintDate").val(formatDate(response.model.ComplaintDate));
            $("#txtStatus").val(response.model.Status);
            $("#txtResolvedBy").val(response.model.ResolvedBy);
            $("#txtResolvedDate").val(formatDate(response.model.ResolveDate));
            $("#txtProof").val(response.model.Proof);
            GetComplaint();
        }
    });
}
// Function to convert "dd-MM-yyyy" to "yyyy-MM-dd"
function formatDate(dateStr) {
    let parts = dateStr.split("/"); // Assuming input format is "dd/MM/yyyy"

    let day = parts[1].padStart(2, '0');   // Ensure two-digit day
    let month = parts[0].padStart(2, '0'); // Ensure two-digit month
    let year = parts[2];  // Year remains unchanged

    return `${year}-${month}-${day}`;  // Convert to "yyyy-MM-dd"
}
//DetailCode


var ComplaintDetail = function (Id) {
    debugger;
    var model = { Id: Id }
    $.ajax({
        url: "/Complaint/ComplaintDetail",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $("#ComplaintModal").modal('show');

            $("#ComplaintDetail").empty();

            var html = "";
            html += "<div class='row'>";
            html += "<div class='col-sm-6'>";
            html += "<b>Id:</b>&nbsp&nbsp&nbsp<span>" + response.model.Id + "</span>";
            html += "</br>";
            html += "<b>FlatNo:</b>&nbsp&nbsp&nbsp<span>" + response.model.FlatNo + "</span>";
            html += "</br>";
            html += "<b>Title:</b>&nbsp&nbsp&nbsp<span>" + response.model.Title + "</span>";
            html += "</br>";
            html += "<b>Description:</b>&nbsp&nbsp&nbsp<span>" + response.model.Description + "</span>";
            html += "</br>";
            html += "<b>Complaint:</b>&nbsp&nbsp&nbsp<span>" + response.model.Complaint + "</span>";
            html += "</br>";
            html += "<b>ComplaintDate:</b>&nbsp&nbsp&nbsp<span>" + response.model.ComplaintDate + "</span>";
            html += "</br>";
            html += "<b>Status:</b>&nbsp&nbsp&nbsp<span>" + response.model.Status + "</span>";
            html += "</br>";
            html += "<b>ResolvedBy:</b>&nbsp&nbsp&nbsp<span>" + response.model.ResolvedBy + "</span>";
            html += "</br>";
            html += "<b>ResolveDate:</b>&nbsp&nbsp&nbsp<span>" + response.model.ResolveDate + "</span>";
            html += "</br>";
            html += "</div>";
            html += "<div class='col-sm-6'>";
            html += "<b>Proof:</b>&nbsp&nbsp&nbsp<span><img src='../Content/Img/" + response.model.Proof + "'style='width:150px;height:180px;'/> </span > ";
            html += "</br>";
            html += "</div>";
            html += "</div>";

            $("#ComplaintDetail").append(html);
        }
    });
};


var ddlComplaintFlatno = function () {
    debugger;
    $.ajax({
        url: "/Resident/GetResident",
        method: "post",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var html = "";
            $("#tblComplaint tbody").empty();
            html += "<option value='1'>Select Flat No </option>";

            $.each(response.message, function (ComplaintIndex, elementvalue) {

                html += "<option value='" + elementvalue.FlatNo + "'>" + elementvalue.FlatNo + "</option>";
            });
            $("#txtFlatNo").append(html);
        }

    });

}

var GetDashboardComplaint = function () {
    debugger;
    $.ajax({
        url: "/Complaint/GetComplaint",
        method: "post",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        async: false,
        success: function (response) {
            var html = "";

            $("#DashbordComplaint tbody").empty();
            $.each(response.model, function (index, elementValue) {
                html += "<div class='news''>";
                html += "<div class='post-item clearfix'>";
                html += "<img src='../Content/Img/" + elementValue.Proof + "' alt=''>";
                html += "<h4>Flat No.    " + elementValue.FlatNo + "</h4>"
                html += "<h4 id='Complaints'> Title:-       " + elementValue.Complaint + "</h4>";
                html += "<h4 id =Complaints'>Status:-       " + elementValue.Status + "</h4>";
                html += "</div>";
                html += "<hr>";
                html += "</div>";

            });
            $("#DashbordComplaint").append(html);


        }
    });
}
