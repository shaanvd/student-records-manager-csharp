const apiUrl = '/api/students';
let editingStudentId = null;


async function updateView() {
    const search = document.getElementById('searchInput').value;
    const sort = document.getElementById('sortSelect').value;

    try {
 
        const response = await fetch(`${apiUrl}?search=${encodeURIComponent(search)}&sortBy=${sort}`);
        window.studentData = await response.json();
        renderStudents(window.studentData);

        const summaryResponse = await fetch(`${apiUrl}/summary`);
        if (summaryResponse.ok) {
            const summaryData = await summaryResponse.json();
            const summaryList = document.getElementById('summaryList');
            if (summaryList) {
                summaryList.innerHTML = '';
                for (const [course, count] of Object.entries(summaryData)) {
                    summaryList.innerHTML += `<li><strong>${course}:</strong> ${count} student(s)</li>`;
                }
            }
        }
    } catch (error) {
        console.error('Error updating view:', error);
    }
}


function renderStudents(studentsToRender) {
    const list = document.getElementById('studentList');
    list.innerHTML = '';

    if (studentsToRender.length === 0) {
        list.innerHTML = '<li>No students match your criteria.</li>';
        return;
    }

    studentsToRender.forEach(s => {
        list.innerHTML += `
            <li>
                <span><strong>ID ${s.id}:</strong> ${s.name} (${s.age} yrs) <br> <small>${s.email || ''}</small> - ${s.course}</span>
                <div>
                    <button onclick="editStudent(${s.id})" style="background:#ffc107; color:#000; border:none; padding:5px 10px; border-radius:4px; cursor:pointer; margin-right:5px;">Edit</button>
                    <button class="delete-btn" onclick="deleteStudent(${s.id})">Delete</button>
                </div>
            </li>
        `;
    });
}


document.getElementById('searchInput').addEventListener('input', updateView);
document.getElementById('sortSelect').addEventListener('change', updateView);


function editStudent(id) {
    const student = window.studentData.find(s => s.id === id);
    if (student) {
        document.getElementById('id').value = student.id;
        document.getElementById('id').disabled = true;
        document.getElementById('name').value = student.name;
        document.getElementById('email').value = student.email || '';
        document.getElementById('age').value = student.age;
        document.getElementById('course').value = student.course;

        document.getElementById('formTitle').innerText = 'Edit Student';
        document.getElementById('saveBtn').innerText = 'Update Student';
        document.getElementById('cancelEditBtn').style.display = 'inline-block';

        editingStudentId = id;
    }
}

function cancelEdit() {
    document.getElementById('studentForm').reset();
    document.getElementById('id').disabled = false;
    document.getElementById('formTitle').innerText = 'Add New Student';
    document.getElementById('saveBtn').innerText = 'Save Student';
    document.getElementById('cancelEditBtn').style.display = 'none';

    editingStudentId = null;
}

document.getElementById('studentForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const data = {
        id: parseInt(document.getElementById('id').value),
        name: document.getElementById('name').value,
        email: document.getElementById('email').value,
        age: parseInt(document.getElementById('age').value),
        course: document.getElementById('course').value
    };

    if (editingStudentId !== null) {
        const response = await fetch(`${apiUrl}/${editingStudentId}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        if (response.ok) {
            cancelEdit();
            updateView(); 
        } else {
            alert('Error updating student.');
        }
    } else {
        const response = await fetch(apiUrl, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        if (response.ok) {
            document.getElementById('studentForm').reset();
            updateView(); 
        } else {
            const errText = await response.text();
            alert('Error: ' + errText);
        }
    }
});


async function deleteStudent(id) {
    if (!confirm(`Are you sure you want to delete Student ${id}?`)) return;

    const response = await fetch(`${apiUrl}/${id}`, {
        method: 'DELETE'
    });

    if (response.ok) {
        updateView(); 
    } else {
        alert('Failed to delete student.');
    }
}

updateView();