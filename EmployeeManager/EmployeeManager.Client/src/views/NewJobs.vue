<template>
    <div class="new-job">
      <h1>Neuen Auftrag anlegen</h1>
      <form @submit.prevent="submitJob" class="job-form">
        <div class="field">
          <label for="customer">Kunde</label>
          <Dropdown id="customer" :options="customers" optionLabel="name" v-model="job.customerId" required />
        </div>
        <div class="field">
          <label for="employee">Mitarbeiter</label>
          <Dropdown id="employee" :options="employees" optionLabel="name" v-model="job.employeeId" required />
        </div>
        <div class="field">
          <label for="datetime">Datum und Uhrzeit</label>
          <Calendar id="datetime" v-model="job.dateTime" showTime required />
        </div>
        <Button label="Erstellen" type="submit" class="submit-button" />
      </form>
      <div v-if="jobs.length" class="jobs-list">
        <h2>Aufträge</h2>
        <DataTable :value="jobs" class="job-table">
          <Column field="customer.name" header="Kunde" />
          <Column field="employee.firstname" header="Mitarbeiter" />
          <Column field="dateTime" header="Datum und Uhrzeit" />
        </DataTable>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  import Dropdown from 'primevue/dropdown';
  import Calendar from 'primevue/calendar';
  import Button from 'primevue/button';
  import DataTable from 'primevue/datatable';
  import Column from 'primevue/column';
  
  const job = ref({
    customerId: null,
    employeeId: null,
    dateTime: null,
  });
  
  const customers = ref([]);
  const employees = ref([]);
  const jobs = ref([]);
  
  const fetchCustomers = async () => {
    try {
      const response = await axios.get('/customers');
      customers.value = response.data;
    } catch (error) {
      console.error('Fehler beim Laden der Kunden:', error);
    }
  };
  
  const fetchEmployees = async () => {
    try {
      const response = await axios.get('/employees');
      employees.value = response.data;
    } catch (error) {
      console.error('Fehler beim Laden der Mitarbeiter:', error);
    }
  };
  
  const fetchJobs = async () => {
    try {
      const response = await axios.get('/jobs');
      jobs.value = response.data;
    } catch (error) {
      console.error('Fehler beim Laden der Aufträge:', error);
    }
  };
  
  const submitJob = async () => {
    try {
      console.log('Sende Daten an API:', {
        customerId: job.value.customerId,
        employeeId: job.value.employeeId,
        dateTime: job.value.dateTime,
      });
      const response = await axios.post('/jobs', {
        customerId: job.value.customerId,
        employeeId: job.value.employeeId,
        dateTime: job.value.dateTime,
      });
      console.log('API-Antwort:', response.data);
      jobs.value.push(response.data);
      job.value = {
        customerId: null,
        employeeId: null,
        dateTime: null,
      };
      alert('Auftrag erfolgreich erstellt');
    } catch (error) {
      console.error('Fehler beim Erstellen des Auftrags:', error.response?.data || error.message);
      alert(`Fehler beim Erstellen des Auftrags: ${error.response?.data?.message || error.message}`);
    }
  };
  
  onMounted(() => {
    fetchCustomers();
    fetchEmployees();
    fetchJobs();
  });
  </script>
  
  <style scoped>
  .new-job {
    max-width: 600px;
    margin: 0 auto;
    padding: 20px;
    background-color: #f9f9f9;
    border-radius: 8px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  }
  
  .field {
    margin-bottom: 1em;
  }
  
  .job-form {
    display: flex;
    flex-direction: column;
  }
  
  .submit-button {
    align-self: flex-end;
    margin-top: 1em;
  }
  
  .jobs-list {
    margin-top: 2em;
  }
  
  .job-table {
    margin-top: 1em;
  }
  
  h1, h2 {
    color: #333;
  }
  
  label {
    display: block;
    margin-bottom: 0.5em;
    font-weight: bold;
  }
  
  input, .p-dropdown, .p-calendar {
    width: 100%;
  }
  
  .submit-button {
    background-color: #007bff;
    color: white;
    border: none;
    padding: 10px 20px;
    font-size: 16px;
    cursor: pointer;
  }
  
  .submit-button:hover {
    background-color: #0056b3;
  }
  
  .p-datatable {
    width: 100%;
    border-collapse: collapse;
  }
  
  .p-datatable th, .p-datatable td {
    padding: 10px;
    border: 1px solid #ddd;
  }
  
  .p-datatable th {
    background-color: #f4f4f4;
    text-align: left;
  }
  </style>
  