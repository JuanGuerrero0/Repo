// person.js - Funcionalidad específica para la gestión de personas

document.addEventListener('DOMContentLoaded', function () {
    // Inicializar DataTable
    const personTable = $('#personTable').DataTable({
        language: {
            // url: 'https://cdn.jsdelivr.net/npm/datatables.net-plugins/i18n/es-ES.json'
        },
        columns: [
            { data: 'id' },
            { data: 'firstName' },
            { data: 'lastName' },
            { data: 'documentType' },
            { data: 'document' },
            { 
                data: 'dateBorn',
                render: function(data) {
                    return formatDate(data);
                }
            },
            { data: 'phoneNumber' },
            { data: 'eps' },
            { data: 'genero' },
            { 
                data: null,
                render: function (data, type, row) {
                    return `
                        <button class="btn btn-sm btn-warning edit-person" data-id="${row.id}">
                            <i class="bi bi-pencil"></i>
                        </button>
                        <button class="btn btn-sm btn-danger delete-person" data-id="${row.id}">
                            <i class="bi bi-trash"></i>
                        </button>
                    `;
                }
            }
        ]
    });
 
    // Cargar datos iniciales
    loadPerson();

    // Event Listeners
    document.getElementById('savePerson').addEventListener('click', savePerson);
    
    // Delegación de eventos para botones de editar y eliminar
    document.getElementById('personTable').addEventListener('click', function(e) {
        if (e.target.closest('.edit-person')) {
            const id = e.target.closest('.edit-person').getAttribute('data-id');
            editPerson(id);
        }
        
        if (e.target.closest('.delete-person')) {
            const id = e.target.closest('.delete-person').getAttribute('data-id');
            showDeleteConfirmation(id);
        }
    });

    // Limpiar modal al abrir para crear nuevo
    $('#personModal').on('show.bs.modal', function (e) {
        if (e.relatedTarget) {
            document.getElementById('personModalLabel').textContent = 'Nueva Persona';
            resetForm('personForm');
        }
    });

    // Confirmación de eliminación
    document.getElementById('confirmDelete').addEventListener('click', deletePerson);
});

// Cargar todas las personas
function loadPerson() {
    fetch(`${API_BASE_URL}/Person`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudieron cargar los datos');
            }
            return response.json();
        })
        .then(data => {
            const table = $('#personTable').DataTable();
            table.clear();
            table.rows.add(data);
            table.draw();
        })
        .catch(error => {
            showNotification('Error al cargar los datos: ' + error.message, 'danger');
        });
}

// Guardar persona (crear o actualizar)
function savePerson() {
    if (!validateForm('personForm')) {
        return;
    }

    const personId = document.getElementById('personId').value;
    const isUpdate = personId !== '' && personId !== null;
    const personData = serializeFormToJson('personForm');
    
    const url = isUpdate ? `${API_BASE_URL}/Person/${personId}` : `${API_BASE_URL}/Person`;
    const method = isUpdate ? 'PUT' : 'POST';

    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: personData
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Error al guardar la persona');
        }
        return response.json();
    })
    .then(data => {
        $('#personModal').modal('hide');
        showNotification(isUpdate ? 'Persona actualizada con éxito' : 'Persona creada con éxito');
        loadPerson();
    })
    .catch(error => {
        showNotification('Error: ' + error.message, 'danger');
    });
}

// Cargar datos para edición
function editPerson(id) {
    fetch(`${API_BASE_URL}/Person/${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo cargar la persona');
            }
            return response.json();
        })
        .then(data => {
            document.getElementById('personModalLabel').textContent = 'Editar Persona';
            
            // Llenar el formulario con los datos
            document.getElementById('personId').value = data.id;
            document.getElementById('firstName').value = data.firstName;
            document.getElementById('lastName').value = data.lastName;
            document.getElementById('documentType').value = data.documentType;
            document.getElementById('document').value = data.document;
            
            // Formatear fecha para input date
            if (data.dateBorn) {
                const date = new Date(data.dateBorn);
                const formattedDate = date.toISOString().split('T')[0];
                document.getElementById('dateBorn').value = formattedDate;
            }
            
            document.getElementById('phoneNumber').value = data.phoneNumber;
            document.getElementById('eps').value = data.eps;
            document.getElementById('genero').value = data.genero;
            document.getElementById('relatedPerson').value = data.relatedPerson || '';
            document.getElementById('isDeleted').checked = data.isDeleted;
            
            // Mostrar el modal
            $('#personModal').modal('show');
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        });
}

// Preparar eliminación
let personIdToDelete = null;

function showDeleteConfirmation(id) {
    personIdToDelete = id;
    $('#deleteConfirmModal').modal('show');
}

// Eliminar persona
function deletePerson() {
    if (personIdToDelete) {
        fetch(`${API_BASE_URL}/Person/permanent/${personIdToDelete}`, {
            method: 'DELETE'
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo eliminar la persona');
            }
            $('#deleteConfirmModal').modal('hide');
            showNotification('Persona eliminada con éxito');
            loadPerson();
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        })
        .finally(() => {
            personIdToDelete = null;
        });
    }
}