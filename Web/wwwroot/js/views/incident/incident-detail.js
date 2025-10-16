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

        showLoader($("#updateIncidentAssestmentModal"));

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
        formData.append("IncidentId", document.getElementById("hdnIncidentID").value);


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
                    var openTaskCount = result.asssetDetails.OpenTaskCount;
                    var completedTaskCount = result.asssetDetails.CompletedTaskCount;

                    $("#assessment").find("#openTaskCount").text(openTaskCount);
                    $("#assessment").find("#completedTaskCount").text(completedTaskCount);

                    SwalSuccessAlert("Updated Successfully");

                    // Optional: close modal and refresh table
                    $("#updateIncidentAssestmentModal").modal("hide");

                    var statusID = $("#ddlStatus").val() != "" ? $("#ddlStatus").val() : 0;
                    var ownerId = $("#ddlOwner").val() != "" ? $("#ddlOwner").val() : 0;
                    var step = $("#global_search_value").val() != "" ? $("#global_search_value").val() : "";

                    debugger;

                    GetAssessmentDetails(statusID, ownerId, step);

                    if (result.partials) {
                        $("#div_Attachments").empty().html(result.partials.viewattachment);
                    }

                    hideLoader($("#updateIncidentAssestmentModal"));



                } else {
                    SwalErrorAlert(result.message || "Update failed.");
                    hideLoader($("#updateIncidentAssestmentModal"));
                }
            } else {
                SwalErrorAlert(result.message || "Update failed.");
                hideLoader($("#updateIncidentAssestmentModal"));
            }
            hideLoader($("#updateIncidentAssestmentModal"));
        } catch (error) {
            console.error("Error:", error);
            SwalErrorAlert(result.message || "Update failed.");
            hideLoader($("#updateIncidentAssestmentModal"));
        }
    });

    $(document).off("change", "#fileInputAssestment");
    $(document).on("change", "#fileInputAssestment", function () {
        const $previewContainer = $('#previewContainerAssestment');
        $previewContainer.empty(); // Clear previous previews

        const files = Array.from(this.files); // Convert FileList to array

        files.forEach(file => {
            const reader = new FileReader();

            reader.onload = function (e) {
                const $preview = $('<div class="preview"></div>').css({
                    width: '100px',
                    height: '100px',
                    overflow: 'hidden',
                    border: '1px solid #ddd',
                    borderRadius: '5px',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    marginRight: '8px'
                }).append(`<img src="${e.target.result}" alt="Image Preview" style="max-width:100%; max-height:100%;">`);

                $previewContainer.append($preview);
            };

            reader.readAsDataURL(file);
        });
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

async function ViewAssessmentDetails(id, mainstepId, substepId) {
    try {
        showLoader($("#div_assestment_view_modal"));

        // Send ID as query string
        const response = await fetch(`/IncidentDetail/ViewAssessmentDetails?id=${id}&mainstepId=${mainstepId}&substepId=${substepId}`, {
            method: "GET",
            headers: {
                "Accept": "text/html"
            }
        });

        if (!response.ok) throw new Error("Failed to load incident details");

        const content = await response.text();
        $("#div_assestment_view_modal").empty().html(content);
        $("#viewIncidentAssestmentModal").modal("show");

    } catch (error) {
        console.error("Error loading incident details:", error);
    } finally {
        hideLoader($("#div_assestment_details"));
    }
}
async function OpenIncidentMap(id) {
    try {
        let payload = { id: id };

        showLoader($(".main-content"));

        const url = `/Incidents/GetIncidentMapDetailsbyId?id=${id}`;

        const response = await fetch(url, {
            method: "GET",
            headers: {
                "Accept": "text/html"
            }
        });

        if (!response.ok) throw new Error("Failed to load incident map");

        const content = await response.text();
        $("#incidentMapContainer").empty().html(content); // 👈 replace with your target div
        $("#MapIncidentModal").modal("show");

    } catch (error) {
        console.error("Error loading incident map:", error);
    } finally {
        hideLoader($(".main-content"));
    }
}