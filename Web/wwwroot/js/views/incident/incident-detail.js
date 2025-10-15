$(function () {

    $(document).off("change", "#ddlStatus, #ddlOwner");
    $(document).on("change", "#ddlStatus, #ddlOwner", function (e) {

        var statusID = $("#ddlStatus").val() != "" ? $("#ddlStatus").val() : 0;
        var ownerId = $("#ddlOwner").val() != "" ? $("#ddlOwner").val() : 0;
        var step = $("#global_search_value").val() != "" ? $("#global_search_value").val() : "";

        e.preventDefault();
        GetAssessmentDetails(statusID, ownerId, step);
    });

    $(document).off("keyup", "#global_search_value");
    $(document).on("keyup", "#global_search_value", function (e) {
        var step = $(this).val().trim();
        var statusID = $("#ddlStatus").val() != "" ? $("#ddlStatus").val() : 0;
        var ownerId = $("#ddlOwner").val() != "" ? $("#ddlOwner").val() : 0;

        e.preventDefault();

        if (step.length >= 3) {
            GetAssessmentDetails(statusID, ownerId, step);
        }
        else {
            GetAssessmentDetails(statusID, ownerId, "");
        }
    });

    $(document).off("click", "#btnUpdateAssessment");
    $(document).on("click", "#btnUpdateAssessment", async function (e) {
        debugger
        e.preventDefault();

        const formData = new FormData();

        // Collect basic fields
        formData.append("Id", document.getElementById("assessmentId").value);
        formData.append("StatusId", document.getElementById("status").value);
        formData.append("AssigneeId", document.getElementById("assignee").value);
        formData.append("StartedTime", document.getElementById("startedTime").value);
        formData.append("CompletedTime", document.getElementById("completedTime").value);
        formData.append("Description", document.getElementById("description").value);
        formData.append("MainStepId", document.getElementById("mainstepId").value);
        formData.append("SubStepId", document.getElementById("substepId").value);

        // Append files (multiple)
        const files = document.getElementById("fileInputAssestment").files;
        for (let i = 0; i < files.length; i++) {
            formData.append("Files", files[i]);
        }

        try {
            const response = await fetch("/IncidentDetail/UpdateAssessment", {
                method: "POST",
                body: formData
            });

            if (response.ok) {
                const result = await response.json();
                if (result.success) {

                    SwalSuccessAlert("Updated Successfully");
                   
                    // Optional: close modal and refresh table
                    $("#updateIncidentAssestmentModal").modal("hide");

                    var statusID = $("#ddlStatus").val() != "" ? $("#ddlStatus").val() : 0;
                    var ownerId = $("#ddlOwner").val() != "" ? $("#ddlOwner").val() : 0;
                    var step = $("#global_search_value").val() != "" ? $("#global_search_value").val() : "";

                    GetAssessmentDetails(statusID, ownerId, step);

                } else {
                    SwalErrorAlert(result.message || "Update failed.");
                }
            } else {
                SwalErrorAlert(result.message || "Update failed.");
            }
        } catch (error) {
            console.error("Error:", error);
            SwalErrorAlert(result.message || "Update failed.");
        }
    });


});

async function GetAssessmentDetails(statusID, ownerId, step) {
    try {


        let payload = {
            IncidentId: $("#hdnIncidentID").val(),
            step: step,
            statusID: statusID,
            ownerId: ownerId
        };

        showLoader($("#div_assestment_details"));

        const response = await fetch("/IncidentDetail/GetAssessmentDetails", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
            body: JSON.stringify(payload)
        });

        if (!response.ok) throw new Error("Failed to load incident list");

        const content = await response.text();
        $("#div_assestment_details").empty().html(content);

    } catch (error) {
        console.error("Error loading incident list:", error);
    } finally {
        hideLoader($("#div_assestment_details"));
    }
}

async function EditAssessmentDetails(id, mainstepId, substepId) {
    try {
        showLoader($("#div_assestment_details"));

        // Send ID as query string
        const response = await fetch(`/IncidentDetail/EditAssessmentDetails?id=${id}&mainstepId=${mainstepId}&substepId=${substepId}`, {
            method: "GET",
            headers: {
                "Accept": "text/html"
            }
        });

        if (!response.ok) throw new Error("Failed to load incident details");

        const content = await response.text();
        $("#div_assestment_modal").empty().html(content);
        $("#updateIncidentAssestmentModal").modal("show");

    } catch (error) {
        console.error("Error loading incident details:", error);
    } finally {
        hideLoader($("#div_assestment_details"));
    }
}